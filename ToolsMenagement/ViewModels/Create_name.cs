using System;

namespace ToolsMenagement.ViewModels;

public class Create_name
{
    public static string Tool_Name(string Opis, double Srednica,string Material,string Przeznaczenie)
    {
        string name = "";
        double tmp = (Srednica / 2);
        name= $"{Opis}, D {Srednica}, {Material}, ";
        if (Przeznaczenie == "Stal")
        {
            name += "Z-S";
        }

        if (Przeznaczenie == "Nieżelazne")
        {
            name += "Z-NZ";
        }
        if (Przeznaczenie == "Drewno")
        {
            name += "Z-D";
        }
        if (Przeznaczenie == "Tworzywa sztuczne")
        {
            name += "Z-TW";
        }

        if (Opis == "Frez walcowy z łbem kulistym ")
        {
            name += $", R={tmp.ToString()}";
        }
        return name;
    }
}