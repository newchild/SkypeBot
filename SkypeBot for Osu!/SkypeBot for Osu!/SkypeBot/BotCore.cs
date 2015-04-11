using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeBot_for_Osu_.SkypeBot
{
	class BotCore
	{
		public event MessageHandler onMessageReceived;
		public delegate void MessageHandler(ChatMessage pMessage, TChatMessageStatus Status);
		private Skype Core;
		private bool connected;
		private string CurrentStatus;
		long LastTick =  DateTime.Now.Ticks;
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
			Core.UserAuthorizationRequestReceived += Core_UserAuthorizationRequestReceived;
		}

		void Core_UserAuthorizationRequestReceived(User pUser)
		{
				
		}

		

		void Core_CallStatus(Call pCall, TCallStatus Status)
		{
			
		}

		void onMessageReceivedEvent(ChatMessage pMessage, TChatMessageStatus Status)
		{
			if (DateTime.Now.Ticks < LastTick + 300000)
				return;
			if (pMessage.Sender.Handle != Core.CurrentUser.Handle)
			{
				onMessageReceived(pMessage, Status);
				LastTick = DateTime.Now.Ticks;
			}
				
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
		
	}
}
