namespace Wlst.Cr.Core.Modularity
{
    //[PetaPoco.TableName("module_config_onload")]
    //[PetaPoco.PrimaryKey("module_id")]
    //[PetaPoco.ExplicitColumns]
    public class ModulesInfo
    {
  
        public int module_id { get; set; }

    
        public string module_name { get; set; }

   
        public bool should_load_onrun { get; set; }
    }
}
