using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeBot_for_Osu_.SkypeBot
{
	class BotCore
	{
		long delay = 1000000;
		public event MessageHandler onMessageReceived;
		public delegate void MessageHandler(ChatMessage pMessage, TChatMessageStatus Status);
		public event CallHandler onCallReceived;
		public delegate void CallHandler(Call pCall, TCallStatus Status);
		private Skype Core;
		private bool connected;
		private string CurrentStatus;
		public BotCore()
		{
			Core = new Skype();
			if (!Core.Client.IsRunning)
				Core.Client.Start(true, true);
			Core.Attach();
			var status = ((ISkype)Core).AttachmentStatus;
			while (!(status == TAttachmentStatus.apiAttachSuccess) && !(status ==TAttachmentStatus.apiAttachRefused))
			{
				CurrentStatus = "Awaiting connection...";
			}
			switch (status)
			{
				case TAttachmentStatus.apiAttachSuccess:
					CurrentStatus = "Connection successful!";
					connected = true;
					break;
				case TAttachmentStatus.apiAttachRefused:
					CurrentStatus = "Connection failed!";
					connected = false;
					break;
				default:
					CurrentStatus = "Unknown Error";
					connected = false;
					break;
			}
			if (!connected)
				throw new Exception("Could not connect");
			Core.MessageStatus += onMessageReceivedEvent;
			Core.CallStatus += Core_CallStatus;
			Core.CallStatus+=Core_CallStatus;
		}


		

		void Core_CallStatus(Call pCall, TCallStatus Status)
		{
			if(Status == TCallStatus.clsRinging)
				onCallReceived(pCall, Status);
		}

		void onMessageReceivedEvent(ChatMessage pMessage, TChatMessageStatus Status)
		{	
			if (pMessage.Sender.Handle != Core.CurrentUser.Handle && Status == TChatMessageStatus.cmsReceived)
			{
				onMessageReceived(pMessage, Status);
				
			}
				
		}


		public void declineCall(Call myCall)
		{
			myCall.Finish();
		}

		public void acceptCall(Call myCall)
		{
			myCall.Answer();
		}
		
		 public void sendMessageToChat(Chat chat, string Text)
		 {
			 chat.SendMessage(Text);
		 }

		 public void sendMessage(string User, string Message)
		 {
			 Core.SendMessage(User, Message);
		 }
		 public string getNickName()
		 {
			 return Core.CurrentUser.Handle;
		 }

		 public void setStatus(string Status)
		 {
			 switch(Status.ToLower()){
				 case "away":
					 Core.CurrentUserStatus = TUserStatus.cusAway;
					 break;
				 case "dnd":
					 Core.CurrentUserStatus = TUserStatus.cusDoNotDisturb;
					 break;
				 case "online":
					 Core.CurrentUserStatus = TUserStatus.cusOnline;
					 break;
				 case "hidden":
					 Core.CurrentUserStatus = TUserStatus.cusInvisible;
					 break;
				 case "offline":
					 Core.CurrentUserStatus = TUserStatus.cusOffline;
					 break;
			 }
		 }

		 public bool isInCall()
		 {
			 foreach (Call item in Core.ActiveCalls)
			 {
				 if (item != null)
					 return true;
			 }
			 return false;
		 }
		
	}
}
