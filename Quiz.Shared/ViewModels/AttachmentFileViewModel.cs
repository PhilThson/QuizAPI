using System;
using System.ComponentModel;

namespace Quiz.Shared.ViewModels
{
	public class AttachmentFileViewModel : AttachmentViewModel
	{
        [DisplayName("Zawartość")]
        public byte[] Content { get; set; }
    }
}