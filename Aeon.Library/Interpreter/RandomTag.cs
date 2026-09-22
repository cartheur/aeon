//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The random element normally returns one of its contained li elements randomly. A list item may opt into
    /// deterministic feedback selection with mood="ParentFeeling:ChildMood", trajectory="repeat", and/or characteristic="Characteristic".
    /// </summary>
    public class RandomTag : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public RandomTag(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
            : base(aeon, thisParticipant, participantQuery, participantRequest, participantResult, templateNode)
        {
            IsRecursive = false;
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The resulting processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "random")
            {
                if (TemplateNode.HasChildNodes)
                {
                    // Only grab <li> nodes.
                    List<XmlNode> listNodes = new List<XmlNode>();
                    foreach (XmlNode childNode in TemplateNode.ChildNodes)
                    {
                        if (childNode.Name == "li")
                        {
                            listNodes.Add(childNode);
                        }
                    }
                    if (listNodes.Count > 0)
                    {
                        if (!listNodes.Any(HasFeedbackConstraint))
                        {
                            return listNodes[Random.Shared.Next(listNodes.Count)].InnerXml;
                        }

                        ResponseVariant[] variants = listNodes.Select(CreateVariant).ToArray();
                        FeedbackSelection selection = FeedbackResponseSelector.Select(
                            variants,
                            ThisAeon.Mood,
                            ThisParticipant.TrajectoryHistory,
                            ParticipantRequest.RawInput,
                            ThisParticipant.LastAeonReply?.InstructionalDisplacement);
                        return selection?.Variant.Content ?? string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        private static bool HasFeedbackConstraint(XmlNode node)
        {
            return node.Attributes?["mood"] != null || node.Attributes?["trajectory"] != null;
        }

        private static ResponseVariant CreateVariant(XmlNode node)
        {
            string moodConstraint = node.Attributes?["mood"]?.Value;
            string trajectoryConstraint = node.Attributes?["trajectory"]?.Value;
            string characteristicConstraint = node.Attributes?["characteristic"]?.Value;
            bool requiresRepeatedTrajectory = false;
            if (!string.IsNullOrWhiteSpace(trajectoryConstraint))
            {
                if (!string.Equals(trajectoryConstraint, "repeat", StringComparison.OrdinalIgnoreCase))
                {
                    throw new XmlException("The random li trajectory attribute must be 'repeat'.");
                }
                requiresRepeatedTrajectory = true;
            }
            if (string.IsNullOrWhiteSpace(moodConstraint))
            {
                return new ResponseVariant(node.InnerXml, requiresRepeatedTrajectory: requiresRepeatedTrajectory, requiredCharacteristic: ParseCharacteristic(characteristicConstraint));
            }

            string[] parts = moodConstraint.Split(':', StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || !Enum.TryParse(parts[0], true, out ParentFeeling parentFeeling))
            {
                throw new XmlException("The random li mood attribute must be 'ParentFeeling:ChildMood'.");
            }
            return new ResponseVariant(node.InnerXml, parentFeeling, parts[1], requiresRepeatedTrajectory, ParseCharacteristic(characteristicConstraint));
        }

        private static Characteristic? ParseCharacteristic(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (!Enum.TryParse(value, true, out Characteristic characteristic))
            {
                throw new XmlException("The random li characteristic attribute must name a Characteristic value.");
            }
            return characteristic;
        }
    }
}
