using Aeon.Library;

var aeon = new global::Aeon.Library.Aeon("1+2i");
aeon.DefaultPredicates.AddSetting("topic", "*");

var firstParticipant = new Participant("first", aeon);
var secondParticipant = new Participant("second", aeon);
firstParticipant.Predicates.UpdateSetting("topic", "private-topic");

Assert(firstParticipant.Topic == "private-topic", "A participant should retain its own predicate changes.");
Assert(secondParticipant.Topic == "*", "Participant predicate state must not leak between conversations.");

aeon.GlobalSettings.AddSetting("notacceptinguserinputmessage", "busy");
aeon.IsAcceptingInput = false;
Assert(aeon.Chat("hello", "third").Output == "busy. ", "The configured unavailable-input message should be returned.");

Assert(
    AlonePrompt.Format("Aeon", "Are you there?") == "Aeon: Are you there?",
    "An alone-state message should be formatted as a participant-facing prompt.");

var trajectoryHistory = new TrajectoryHistory(capacity: 2);
trajectoryHistory.Add(new TrajectoryIndication { RawInput = "first", RecordedAtUtc = DateTime.UtcNow });
trajectoryHistory.Add(new TrajectoryIndication { RawInput = "second", RecordedAtUtc = DateTime.UtcNow });
trajectoryHistory.Add(new TrajectoryIndication { RawInput = "third", RecordedAtUtc = DateTime.UtcNow });
Assert(trajectoryHistory.Indications.Count == 2, "Trajectory history should retain only its configured capacity.");
Assert(trajectoryHistory.Indications[0].RawInput == "second", "Trajectory history should discard the oldest indication first.");
Assert(trajectoryHistory.Indications[1].Sequence == 3, "Trajectory indication sequence numbers should remain monotonic.");

var indicationRequest = new ParticipantRequest("record this", firstParticipant, aeon);
var indicationResult = new ParticipantResult(firstParticipant, aeon, indicationRequest, "1+2i");
indicationResult.NormalizedTrajectories.Add("RECORD THIS <THAT> * <TOPIC> *");
indicationResult.OutputSentences.Add("Recorded");
indicationResult.ReturnIndication();
Assert(indicationResult.TrajectoryIndication.RawInput == "record this", "ReturnIndication should retain the raw participant input.");
Assert(indicationResult.TrajectoryIndication.Paths.Count == 1, "ReturnIndication should retain normalized paths.");
Assert(firstParticipant.TrajectoryHistory.Indications.Count == 1, "ReturnIndication should add the indication to the participant history.");

string trajectoryTestDirectory = Path.Combine(Path.GetTempPath(), "aeon-trajectory-smoke-" + Guid.NewGuid().ToString("N"));
string trajectoryTestPath = Path.Combine(trajectoryTestDirectory, "history.json");
try
{
    trajectoryHistory.Save(trajectoryTestPath);
    var reloadedHistory = new TrajectoryHistory(capacity: 2);
    Assert(reloadedHistory.Load(trajectoryTestPath), "A persisted trajectory history should reload.");
    Assert(reloadedHistory.Indications.Count == 2, "Reloaded trajectory history should retain every saved indication.");
    Assert(reloadedHistory.Indications[1].RawInput == "third", "Reloaded trajectory history should preserve indication contents.");
}
finally
{
    if (Directory.Exists(trajectoryTestDirectory))
    {
        Directory.Delete(trajectoryTestDirectory, true);
    }
}

var otherAeon = new global::Aeon.Library.Aeon("1+2i");
try
{
    aeon.Chat(new ParticipantRequest("hello", new Participant("other", otherAeon), otherAeon));
    throw new InvalidOperationException("A request for another aeon should be rejected.");
}
catch (ArgumentException)
{
}

try
{
    AlonePrompt.Format("Aeon", " ");
    throw new InvalidOperationException("A blank alone-state message should be rejected.");
}
catch (ArgumentException)
{
}

Console.WriteLine("Aeon library smoke tests passed.");

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}
