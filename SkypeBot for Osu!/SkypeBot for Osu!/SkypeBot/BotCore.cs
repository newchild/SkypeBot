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
		public event MessageHandler onMessageReceived;
		public delegate void MessageHandler(ChatMessage pMessage, TChatMessageStatus Status);
		private Skype Core;
		private bool connected;
		private string CurrentStatus;
		public BotCore()
		{
			Core = new Skype();
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

		}

		 void onMessageReceivedEvent(ChatMessage pMessage, TChatMessageStatus Status)
		{
			if (pMessage.Sender != Core.User && Status == TChatMessageStatus.cmsReceived)
			{
				
				onMessageReceived(pMessage, Status);
			}
				
		}
		
	}
}
