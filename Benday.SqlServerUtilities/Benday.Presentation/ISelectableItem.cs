using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public interface ISelectableItem : ISelectable
    {        
        string Text { get; set; }
        string Value { get; set; }
        int Id { get; set; }
    }
}