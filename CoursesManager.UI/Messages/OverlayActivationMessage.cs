using CoursesManager.MVVM.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Messages
{
    class OverlayActivationMessage : BaseMessage<OverlayActivationMessage>
    {
        public bool IsVisible { get; set; }

        public OverlayActivationMessage(bool isVisible) 
        {
            IsVisible = isVisible;
        }


        public override OverlayActivationMessage Clone()
        {
            return new OverlayActivationMessage(IsVisible)
            {
                IsVisible = IsVisible,
                MessageId = MessageId,
                TimeStamp = new(TimeStamp.Ticks)
            };
        }
    }
}
