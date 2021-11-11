using System;
using System.Collections;
using Newtonsoft.Json;

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
    /// jqGridDataFrame stores all the data send back as a response to jqGrid AJAX data request.
    /// </summary>
    /// <remarks>Create an instance of this class providing all the data required by the constructors.
    /// Then use the <see cref="M:Codemerx.jqGrid.Tools.jqGridDataFrame.GetJSON"/> method to get the data back n JSON format.
    /// The resulting string can then be sent back as a reply to jqGri AJAX data request.</remarks>
    public class jqGridDataFrame
    {
        //private static readonly int MAXJSONRESPONSELENGTH = Int32.MaxValue;

        /// <summary>
        /// The number of data page to display in jqGrid.
        /// </summary>
        public int page;

        /// <summary>
        /// The total number of data records currently bound to jqGrid.
        /// </summary>
        public int records;

        /// <summary>
        /// An array of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/> objects. Each <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/> object represents a single row of
        /// data displayed by jqGrid.
        /// </summary>
        public JSONGridRow[] rows;

        /// <summary>
        /// The total number of pages in jqGrid.
        /// </summary>
        public int total;

        /// <summary>
        /// Used to send custom data together with the response. jqGrid will not automatically display that data. It could be accessed by custome javascrpt code
        /// on the client side, though.
        /// </summary>
        public Hashtable userdata;

        /// <summary>
        /// Initializes an instance of <see cref="Codemerx.jqGrid.Tools.jqGridDataFrame"/>
        /// </summary>
        /// <param name="pageNumber">The number of data page to display in jqGrid</param>
        /// <param name="allRowsCount">The total number of data records currently bound to jqGrid.</param>
        /// <param name="pagesCount"></param>
        /// <param name="data"> An array of <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/> objects. Each <see cref="Codemerx.jqGrid.Tools.JSONGridRow"/> object represents a single row of
        /// data displayed by jqGrid.</param>
        /// <param name="customData">Used to send custom data together with the response. jqGrid will not automatically display that data. It could be accessed by custome javascrpt code
        /// on the client side, though.</param>
        public jqGridDataFrame(int pageNumber, int allRowsCount, int pagesCount, JSONGridRow[] data, Hashtable customData)
        {
            this.userdata = customData;
            this.page = pageNumber;
            this.total = pagesCount;
            this.records = allRowsCount;
            this.rows = data;
        }

        /// <summary>
        /// Gets the JSON string that holds all the data in this instance of <see cref="Codemerx.jqGrid.Tools.jqGridDataFrame"/>
        /// </summary>
        /// <returns>The JSON string that holds all the data in this instance of <see cref="Codemerx.jqGrid.Tools.jqGridDataFrame"/></returns>
        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
