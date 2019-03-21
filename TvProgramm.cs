using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApp1

class TvProgramm
{
    public TvProgramm(String Date, string Program)
    {
        this.Date = Date;
        this.Program = Program;
    }
    public string Date { get; set; }
    public string Program { get; set; }
}