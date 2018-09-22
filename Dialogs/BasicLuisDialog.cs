using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        // CONSTANTS        
        // Entity
        public const string Entity_Device = "HomeAutomation.Device";
        public const string Entity_Room = "HomeAutomation.Room";
        public const string Entity_Operation = "HomeAutomation.Operation";
        public const string Entity_TVChannel = "TV.ChannnelName";
        
        // Intents
        public const string Intent_TurnOn = "HomeAutomation.TurnOn";
        public const string Intent_TurnOff = "HomeAutomation.TurnOff";
        public const string Intent_TVGuide = "TV.ShowGuide";
        public const string Intent_TVChannel = "TV.ChangeChannel";
        public const string Intent_TVWatch = "TV.WatchTV";
        public const string Intent_None = "None";
        public const string Intent_Help = "Help";

        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent(Intent_Help)]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_None)]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_TurnOn)]
        public async Task OnIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent(Intent_TurnOff)]
        public async Task OffIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        [LuisIntent(Intent_TVChannel)]
        public async Task TVChannelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        [LuisIntent(Intent_TVGuide)]
        public async Task TVGuideIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        [LuisIntent(Intent_TVWatch)]
        public async Task TVWatchIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            // get recognized entities
            string entities = this.BotEntityRecognition(result);
            
            // round number
            string roundedScore =  result.Intents[0].Score != null ? (Math.Round(result.Intents[0].Score.Value, 2).ToString()) : "0";
            
            if (result.Intents[0].Intent==Intent_Help)
            {
                await context.PostAsync($"These are the kind of tasks you can perform:");
            }
            else
            { 
                await context.PostAsync($"**Query**: {result.Query}, **Intent**: {result.Intents[0].Intent}, **Score**: {roundedScore}. **Entities**: {entities}");
            }
            context.Wait(MessageReceived);
        }
        
        // Entities found in result
        public string BotEntityRecognition(LuisResult result)
        {
            StringBuilder entityResults = new StringBuilder();
        
            if(result.Entities.Count>0)
            {
                foreach (EntityRecommendation item in result.Entities)
                {
                    // Query: Turn on the [light]
                    // item.Type = "HomeAutomation.Device"
                    // item.Entity = "light"
                    entityResults.Append(item.Type + "=" + item.Entity + ",");
                }
                // remove last comma
                entityResults.Remove(entityResults.Length - 1, 1);
            }
        
            return entityResults.ToString();
        }
    }
}