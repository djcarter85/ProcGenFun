namespace ProcGenFun.WinForms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new DistributionsForm());
    }
}