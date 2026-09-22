using Aeon.Library;
using Aeon.Library.Builder;

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

var moodState = new MoodState();
Assert(MoodState.Catalog.Count == 8 && MoodState.Catalog.All(pair => pair.Value.Count == 7), "The mood wheel should retain every Table 1 parent feeling and child mood.");
Assert(moodState.CurrentMood == null && moodState.LastMoodIndices.All(pair => pair.Value == 0), "A new mood wheel should begin with every parent in the off state.");
EmotiveIndication charmed = moodState.SetMood(ParentFeeling.Happy, "charmed");
Assert(charmed.Index == 3 && charmed.Weight > 0 && charmed.Weight < 1, "Selecting a mood should set its parent indicator and emit a normalized indication.");
EmotiveIndication drained = moodState.SetMood(ParentFeeling.Tired, "Drained");
Assert(moodState.LastMoodIndices[ParentFeeling.Happy] == 3 && drained.Index == 7 && drained.Weight == 1, "Mood history should retain parent indicators and give the final wheel position full weight.");
var helpedOnlyMood = new MoodState();
helpedOnlyMood.Randomize(new Random(17), new[] { ParentFeeling.Helped });
Assert(helpedOnlyMood.CurrentMood.ParentFeeling == ParentFeeling.Helped, "A constrained mood selection must stay within the allowed parent-feeling scope.");
try
{
    moodState.SetMood(ParentFeeling.Happy, "Drained");
    throw new InvalidOperationException("A mood must belong to its assigned parent feeling.");
}
catch (ArgumentException)
{
}

var feedbackHistory = new TrajectoryHistory();
feedbackHistory.Add(new TrajectoryIndication { RawInput = "Tell me more" });
var feedbackMood = new MoodState();
feedbackMood.SetMood(ParentFeeling.Happy, "Charmed");
FeedbackSelection feedbackSelection = FeedbackResponseSelector.Select(
    new[]
    {
        new ResponseVariant("fallback"),
        new ResponseVariant("mood", ParentFeeling.Happy, "Charmed"),
        new ResponseVariant("repeat", requiresRepeatedTrajectory: true),
        new ResponseVariant("both", ParentFeeling.Happy, "Charmed", requiresRepeatedTrajectory: true)
    },
    feedbackMood,
    feedbackHistory,
    " tell me more ");
Assert(feedbackSelection.Variant.Content == "both" && feedbackSelection.IsRepeatedTrajectory, "Feedback selection should prefer a variant constrained by both current mood and repeated trajectory.");
FeedbackSelection fallbackSelection = FeedbackResponseSelector.Select(
    new[] { new ResponseVariant("fallback"), new ResponseVariant("repeat", requiresRepeatedTrajectory: true) },
    feedbackMood,
    feedbackHistory,
    "new input");
Assert(fallbackSelection.Variant.Content == "fallback", "Feedback selection should use an explicit fallback when no constrained variant is eligible.");
var feedbackAeon = new global::Aeon.Library.Aeon("1+2i");
var feedbackParticipant = new Participant("feedback", feedbackAeon);
feedbackParticipant.TrajectoryHistory.Add(new TrajectoryIndication { RawInput = "repeat me" });
feedbackAeon.Mood.SetMood(ParentFeeling.Happy, "Charmed");
var feedbackRequest = new ParticipantRequest("repeat me", feedbackParticipant, feedbackAeon);
var feedbackResult = new ParticipantResult(feedbackParticipant, feedbackAeon, feedbackRequest, "1+2i");
var feedbackXml = new System.Xml.XmlDocument();
feedbackXml.LoadXml("<random><li>fallback</li><li mood=\"Happy:Charmed\" trajectory=\"repeat\">selected</li></random>");
var feedbackRandomTag = new RandomTag(feedbackAeon, feedbackParticipant, new ParticipantQuery(string.Empty), feedbackRequest, feedbackResult, feedbackXml.DocumentElement);
Assert(feedbackRandomTag.Transform() == "selected", "Annotated random variants should receive mood and trajectory feedback in the interpreter.");

var classifierTrajectory = new TrajectoryIndication { RawInput = "classify this", Paths = new List<string> { "CLASSIFY THIS <THAT> * <TOPIC> *" } };
var classifierMood = new MoodState();
EmotiveIndication classifierEmotive = classifierMood.SetMood(ParentFeeling.Happy, "Charmed");
Assert(
    InstructionalDisplacementClassifier.TryClassify("p(x) = 1 + 3x + x^2 + 2x^3 + y + t", classifierTrajectory, classifierEmotive, TimeSpan.FromSeconds(2), out InstructionalDisplacement displacement),
    "A configured characteristic equation should classify trajectory, emotive, and time coordinates.");
double expectedClassifierValue = 1 + (3 * displacement.TrajectoryCoordinate) + Math.Pow(displacement.TrajectoryCoordinate, 2) + (2 * Math.Pow(displacement.TrajectoryCoordinate, 3)) + classifierEmotive.Weight + 2;
Assert(Math.Abs(displacement.Value - expectedClassifierValue) < 0.0000001 && displacement.EmotiveCoordinate == classifierEmotive.Weight && displacement.TimeCoordinate == 2, "The classifier should evaluate its bounded equation language using x, y, and t.");
Assert(!InstructionalDisplacementClassifier.TryClassify("System.IO.File.Delete(x)", classifierTrajectory, classifierEmotive, TimeSpan.Zero, out _), "Unsupported characteristic equation syntax must be rejected without execution.");

var recordingOutputAdapter = new RecordingOutputAdapter(OutputModality.Text | OutputModality.Animation);
var outputDispatcher = new OutputDispatcher(new[] { recordingOutputAdapter });
var outputPresentation = new OutputPresentation("Aeon", "Hello", OutputModality.Text, classifierEmotive, displacement);
outputDispatcher.Present(outputPresentation);
Assert(recordingOutputAdapter.Presentations.Count == 1 && ReferenceEquals(recordingOutputAdapter.Presentations[0], outputPresentation), "Output dispatch should deliver a contextual response to compatible adapters.");
outputDispatcher.Present(new OutputPresentation("Aeon", "Unsupported", OutputModality.Tactile));
Assert(recordingOutputAdapter.Presentations.Count == 1, "Output dispatch should not send a modality to an incompatible adapter.");

var learningProposal = LearningProposal.Create("  hello there  ", "  Hello back.  ");
Assert(learningProposal.Pattern == "hello there", "Learning should retain a reviewed, trimmed participant phrase.");
Assert(learningProposal.Response == "Hello back.", "Learning should retain a reviewed, trimmed participant response.");
Assert(learningProposal.ToDocument().Root?.Element("category")?.Element("template")?.Value == "Hello back.", "Learning proposals should serialize the literal response as XML text.");
try
{
    LearningProposal.Create("\n", "response");
    throw new InvalidOperationException("A blank learning phrase should be rejected.");
}
catch (ArgumentException)
{
}
try
{
    LearningProposal.Create("hello *", "response");
    throw new InvalidOperationException("A learned phrase must not create a wildcard category.");
}
catch (ArgumentException)
{
}

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

sealed class RecordingOutputAdapter : IOutputAdapter
{
    public RecordingOutputAdapter(OutputModality supportedModalities)
    {
        SupportedModalities = supportedModalities;
    }

    public OutputModality SupportedModalities { get; }
    public List<OutputPresentation> Presentations { get; } = new List<OutputPresentation>();

    public void Present(OutputPresentation presentation)
    {
        Presentations.Add(presentation);
    }
}
