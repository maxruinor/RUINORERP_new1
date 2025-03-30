using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Models
{
    public class ChartInteraction
    {
        public List<ChartContextMenuItem> ContextMenuItems { get; } = new List<ChartContextMenuItem>();

        public void AddMenuItem(string text, Action<ChartData> action)
        {
            ContextMenuItems.Add(new ChartContextMenuItem(text, action));
        }
    }

    public class ChartContextMenuItem
    {
        public string Text { get; private set; }
        public Action<ChartData> Action { get; private set; }

        public ChartContextMenuItem(string text, Action<ChartData> action)
        {
            Text = text;
            Action = action;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ChartContextMenuItem other = (ChartContextMenuItem)obj;
            return Text == other.Text && Action == other.Action;
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0) ^ (Action != null ? Action.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"ChartContextMenuItem {{ Text = {Text}, Action = {Action} }}";
        }
    }
}

