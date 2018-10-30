using System.Collections.Generic;

namespace Helios_PoC
{
    public static class HeliosDefaults
    {
        private const string Aussenluft = "v00104";
        private const string Zuluft = "v00105";
        private const string Fortluft = "v00106";
        private const string Abluft = "v00107";

        public static IEnumerable<VariableDeclaration> GetVariableDeclarations()
        {
            yield return new VariableDeclaration(
                Aussenluft,
                8,
                "Temperatur: Aussenluft",
                AccessMode.R,
                typeof(float),
                "-27",
                "9999");
            yield return new VariableDeclaration(
                Zuluft,
                8,
                "Temperatur: Zuluft",
                AccessMode.R,
                typeof(float),
                "-27",
                "9999");
            yield return new VariableDeclaration(
                Abluft,
                8,
                "Temperatur: Abluft",
                AccessMode.R,
                typeof(float),
                "-27",
                "9999");
            yield return new VariableDeclaration(
                Fortluft,
                8,
                "Temperatur: Fortluft",
                AccessMode.R,
                typeof(float),
                "-27",
                "9999");
        }
    }
}