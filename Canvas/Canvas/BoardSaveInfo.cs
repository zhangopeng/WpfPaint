using System;
using System.Collections.Generic;

namespace Canvas
{
    //[SaveInfoContract("Board")]
    public class BoardSaveInfo : SaveInfo
    {
       // [SaveInfoMember("Slides", IsRequired = true, Description = "页面Id集合")]
        public IList<string> SlideIds { get; set; }
    }
}
