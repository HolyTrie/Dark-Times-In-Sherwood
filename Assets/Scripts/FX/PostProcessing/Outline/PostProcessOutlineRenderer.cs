using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessEffects
{
    public class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutlineSettings>
    {
        private string _path = "Hidden/"; //*Refactor* - support any file path...
        public string Path 
        {
            get{return _path;} 
            set
            { 
                if(value.ElementAt(value.Length-1) == '/')
                    value += "/"; // we want to append '/' if it doesnt exist. 
                _path = value;
            }
        }
        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(Shader.Find($"{_path}{settings.name}")); // property sheet contains the materials we need to alter in our post process
            sheet.properties.SetFloat("_Thickness", settings.thickness);
            sheet.properties.SetFloat("_MinDepth", settings.depthMin); 
            sheet.properties.SetFloat("_MaxDepth", settings.depthMax);  
            //?????
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}