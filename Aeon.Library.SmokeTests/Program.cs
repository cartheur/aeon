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

var otherAeon = new global::Aeon.Library.Aeon("1+2i");
try
{
    aeon.Chat(new ParticipantRequest("hello", new Participant("other", otherAeon), otherAeon));
    throw new InvalidOperationException("A request for another aeon should be rejected.");
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
