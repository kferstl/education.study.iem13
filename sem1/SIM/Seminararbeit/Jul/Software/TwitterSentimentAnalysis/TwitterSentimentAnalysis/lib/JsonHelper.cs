using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Newtonsoft.Json {
	public static class JsonHelper {
		public static JsonSerializer CreateSerializer() {
			var settings = new JsonSerializerSettings();
			settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
			settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
			var serializer = JsonSerializer.Create(settings);
			return serializer;
		}

		public static string Serialize(this JsonSerializer serializer, object o, bool format = false) {
			using (var writer = new StringWriter()) {
				using (var jsonWriter = new JsonTextWriter(writer)) {
					jsonWriter.Formatting = format ? Formatting.Indented : Formatting.None;
					serializer.Serialize(jsonWriter, o);
					return writer.ToString();
				}
			}
		}
		public static async Task<string> SerializeAsync(this JsonSerializer serializer, object o, bool format = false) {
			return await Task<string>.Run(() => serializer.Serialize(o, format));
		}


		public static T Deserialize<T>(this JsonSerializer serializer, string json) {
			using (var reader = new StringReader(json)) {
				using (var jsonReader = new JsonTextReader(reader)) {
					var o = serializer.Deserialize<T>(jsonReader);
					return o;
				}
			}
		}

		public static async Task<T> DeserializeAsync<T>(this JsonSerializer serializer, string json) {
			return await Task<T>.Run(() => serializer.Deserialize<T>(json));
		}

		public static T LoadFromFile<T>(string filePath) {
			var serializer = CreateSerializer();
			using (var reader = new StreamReader(filePath, Encoding.UTF8)) {
				using (var jsonReader = new JsonTextReader(reader)) {
					var o = serializer.Deserialize<T>(jsonReader);
					return o;
				}
			}
		}
		public static Task<T> LoadFromFileAsync<T>(string filePath) {
			return Task<T>.Run(() => LoadFromFile<T>(filePath));
		}

		public static void SaveToFile(object o, string filePath, bool format = false) {
			try {
				var serializer = CreateSerializer();
				if (File.Exists(filePath)) {
					File.Delete(filePath);
				}
				using (var writer = new StreamWriter(filePath, false)) {
					using (var jsonWriter = new JsonTextWriter(writer)) {
						jsonWriter.Formatting = format ? Formatting.Indented : Formatting.None;
						serializer.Serialize(jsonWriter, o);
					}
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
				throw;
			}
		}

		public static async Task SaveToFileAsync(object o, string filePath, bool format = false) {
			await Task.Run(() => SaveToFile(o, filePath, format));
		}
	}
}