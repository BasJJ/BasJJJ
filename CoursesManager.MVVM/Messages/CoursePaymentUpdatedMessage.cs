using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.MVVM.Messages
{
    public class CoursePaymentUpdatedMessage : BaseMessage<CoursePaymentUpdatedMessage>
    {
        public override CoursePaymentUpdatedMessage Clone()
        {
            return new CoursePaymentUpdatedMessage
            {
                MessageId = MessageId,
                TimeStamp = new(TimeStamp.Ticks)
            };
        }
    }
}