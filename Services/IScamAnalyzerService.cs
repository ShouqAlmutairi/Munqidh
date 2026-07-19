using ScamShieldAI.Models;

namespace ScamShieldAI.Services
{
    public interface IScamAnalyzerService
    {
        AnalysisResultViewModel Analyze(string message);
    }
}
