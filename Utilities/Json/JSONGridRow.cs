

//Copyright (C) 2013 Codemerx

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
//to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
//OR OTHER DEALINGS IN THE SOFTWARE.

namespace Utilities.Json
{
    /// <summary>
    /// Represents a jqGrid row.
    /// </summary>
    public class JSONGridRow
    {
        /// <summary>
        /// The unique ID of the jqGrid row represented by this instance of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/>
        /// </summary>
        public string id;

        /// <summary>
        /// An array of objects representing the cell values of the jqGrid row represented by this instance of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/>.
        /// The number of elements in this array should equal the number of columns defined in jqGrid colModel.
        /// </summary>
        public object[] cell;

        /// <summary>
        /// Initilizes a new instance of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/>.
        /// </summary>
        /// <param name="ID">The unique ID of the jqGrid row represented by this instance of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/></param>
        /// <param name="cells">An array of objects representing the cell values of the jqGrid row represented by this instance of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/>.</param>The number of elements in this array should equal the number of columns defined in jqGrid colModel.
        public JSONGridRow(string ID, object[] cells)
        {
            id = ID;
            cell = cells;
        }
    }
}
