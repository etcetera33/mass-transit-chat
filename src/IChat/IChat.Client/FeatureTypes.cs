using System.ComponentModel.DataAnnotations;

namespace IChat.Client
{
    public enum FeatureTypes
    {
        [Display(Name = "SimpleSend")]
        SimpleSend = 1,
        [Display(Name = "SimplePublish")]
        SimplePublish = 2
    }
}