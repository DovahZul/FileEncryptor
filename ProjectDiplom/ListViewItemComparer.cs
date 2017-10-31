using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectDiplom
{
    class ListViewItemComparer : System.Collections.IComparer {
    private int col;
    private SortOrder order;
    public ListViewItemComparer() {
        col=0;
        order = SortOrder.Ascending;
    }
    public ListViewItemComparer(int column, SortOrder order) 
    {
        col=column;
        this.order = order;
    }
    public int Compare(object x, object y) 
    {
        int returnVal= -1;
        returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                ((ListViewItem)y).SubItems[col].Text);
        // Определение того, является ли порядок сортировки порядком "по
        // убыванию".
        if (order == SortOrder.Descending)
            // Изменение значения, возвращенного String.Compare, на
            // противоположное.
            returnVal *= -1;
        return returnVal;
    }
}
}
