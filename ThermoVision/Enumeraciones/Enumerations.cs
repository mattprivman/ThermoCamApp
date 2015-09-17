using System;

namespace ThermoVision.Enumeraciones
{
    public enum  appType : int
    {
        Standart                = 1,
        Gas                     = 2,
        RampControl             = 3
    }

    public enum CameraType : short
    {
        THERMACAM_S40           = 4,
        THERMOVISION_A20        = 5,
        INDIGO_MERLIN           = 6,
        INDIGO_PHOENIX          = 7,
        INDIGO_OMEGA            = 8,
        FLIR_SC4000             = 9,
        THERMACAM_SC640         = 10,
        FLIR_A3X0               = 11,
        FLIR_A3X5               = 12,
        FLIR_GF320              = 13,
        FLIR_T_SERIES           = 14,
        FLIR_A615               = 15
    }
    public enum DeviceType : short
    {
        FileDevice              = 2,
        FireWire                = 3,
        Ethernet8bits           = 4,
        FireWire16bits          = 5,
        Ethernet16bits          = 6,
        EthernetiPort           = 8,
        USB                     = 9
    }
    public enum InterfaceType : short
    {
        NO_CONNECTION_ATTEMPTED = 0,
        TCP                     = 2,
        FireWire                = 3,
        iPort                   = 4,
        AXIS_2401               = 5,
        USB                     = 6
    }
    public enum lookUPTable : short
    {
        _8BitsPixel             = 0,
        _16BitsPixel            = 1,
        _15BitsPixel            = 2
    }
}
