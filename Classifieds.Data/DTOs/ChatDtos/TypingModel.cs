﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Data.DTOs.ChatDtos
{
    public class TypingModel
    {
        public Guid ChatId { get; set; }
        public UserInMessage FromUser { get; set; }
        public List<Guid> ToUserId { get; set; }
        public bool Typing { get; set; }
    }
}
