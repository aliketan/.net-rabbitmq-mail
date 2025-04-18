using Fluid;

namespace App.Application.Service.Concrete
{
    using Contracts;

    public class TemplateEngine : ITemplateEngine
    {
        private readonly FluidParser _parser;

        #region Constructor
        public TemplateEngine(FluidParser parser)
        {
            _parser = parser;
        }
        #endregion

        public async Task<string> ParseAsync(string source, object model)
        {
            if (_parser.TryParse(source, out var template, out var error))
                return await template.RenderAsync(new TemplateContext(model));

            return error;
        }

        public string Parse(string source, object model)
        {
            if (_parser.TryParse(source, out var template, out var error))
                return template.Render(new TemplateContext(model));

            return error;
        }
    }
}
