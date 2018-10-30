namespace Helios_PoC
{

    // see reference: https://www.easycontrols.net/en/service/downloads/send/4-software/16-modbus-dokumentation-f%C3%BCr-kwl-easycontrols-ger%C3%A4te
    public static class HeliosVariables
    {
        public static VariableDeclaration<float> FortluftTemperatur => new VariableDeclaration<float>(
            "v00106",
            8,
            "Temperatur: Fortluft",
            AccessMode.R,
            "-27",
            "9999");

        public static VariableDeclaration<float> AbluftTemperatur => new VariableDeclaration<float>(
            "v00107",
            8,
            "Temperatur: Abluft",
            AccessMode.R,
            "-27",
            "9999");

        public static VariableDeclaration<float> ZuluftTemperatur => new VariableDeclaration<float>(
            "v00105",
            8,
            "Temperatur: Zuluft",
            AccessMode.R,
            "-27",
            "9999");

        public static VariableDeclaration<float> AussenluftTemperatur => new VariableDeclaration<float>(
            "v00104",
            8,
            "Temperatur: Aussenluft",
            AccessMode.R,
            "-27",
            "9999");
    }
}