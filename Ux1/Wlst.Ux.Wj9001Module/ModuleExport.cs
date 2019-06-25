﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.CoreInterface;

namespace Wlst.Ux.Wj9001Module
{
    [Export(typeof(IIEquipmentModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ModuleExport : IIEquipmentModel
    {
        public bool CanBeAttachEquipmnet
        {
            get { return true; }
        }

        public bool CanBeMainEquipment
        {
            get { return false; }
        }


        public int ModelKey
        {
            get { return 9001; }
        }

        public string ModuleDescription
        {
            get { return "4路漏电4路温度"; }
        }


        public EquipmentModelArgs Args
        {
            get
            {
                return new EquipmentModelArgs()
                {
                    ImageSourcePath = "/Image/EquipmentImage/9001.png",
                    ModelInfoSetViewAttachRegion = Cr.Core.CoreServices.DocumentRegionName.DocumentRegion,
                    ModelInfoSetViewId = Services.ViewIdAssign.Wj9001ParaSetViewId,
                    Name = "4路漏电4路温度"

                };
            }
        }

        public string ModelName
        {
            get { return "4路漏电4路温度"; }
        }
    }
}
