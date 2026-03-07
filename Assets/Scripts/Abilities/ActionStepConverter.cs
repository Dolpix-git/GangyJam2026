using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CardGame.Card.CardData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CardGame.Abilities
{
    public class ActionStepConverter : JsonConverter<ActionStepJson>
    {
        private static readonly Dictionary<string, Type> _actionTypes = BuildLookup();

        private static Dictionary<string, Type> BuildLookup()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IAction).IsAssignableFrom(t))
                .ToDictionary(t => t.Name.ToLowerInvariant());
        }

        public override ActionStepJson ReadJson(JsonReader reader, Type objectType, ActionStepJson existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var type = obj["Type"]?.Value<string>();
            var dataToken = obj["Data"];

            IAction action = null;

            if (type != null && _actionTypes.TryGetValue(type.ToLowerInvariant(), out var actionType))
            {
                action = (IAction)dataToken?.ToObject(actionType);
            }
            else
            {
                Debug.LogWarning($"[ActionStepConverter] Unknown action type '{type}'");
            }

            return new ActionStepJson { Action = action };
        }

        public override void WriteJson(JsonWriter writer, ActionStepJson value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
