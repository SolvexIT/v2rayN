using System.IO;
using System.Threading.Tasks;
using ServiceLib.Common;
using ServiceLib.Models;

namespace ServiceLib.Handler;

public static class SolvexITHandler
{
    private static readonly string _fileName = "solvexIT_servers.json";

    public static async Task<SolvexITConfig> LoadConfig()
    {
        try
        {
            var path = Utils.GetConfigPath(_fileName);
            if (!File.Exists(path))
            {
                return new SolvexITConfig();
            }

            var content = await File.ReadAllTextAsync(path);
            var config = JsonUtils.Deserialize<SolvexITConfig>(content);
            return config ?? new SolvexITConfig();
        }
        catch
        {
            return new SolvexITConfig();
        }
    }

    public static async Task SaveConfig(SolvexITConfig config)
    {
        try
        {
            var path = Utils.GetConfigPath(_fileName);
            var content = JsonUtils.Serialize(config, true, true);
            await File.WriteAllTextAsync(path, content);
        }
        catch
        {
            // Log error
        }
    }
}
