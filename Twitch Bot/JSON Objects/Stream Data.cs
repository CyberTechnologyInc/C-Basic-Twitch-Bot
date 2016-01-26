using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.JSON_Objects {
	public class StreamData {
		public class Links {
			public string Self { get; set; }
		}

		public class Preview {
			public string Small { get; set; }
			public string Medium { get; set; }
			public string Large { get; set; }
			public string Template { get; set; }
		}

		public class Links2 {
			public string Self { get; set; }
			public string Follows { get; set; }
			public string Commercial { get; set; }
			public string StreamKey { get; set; }
			public string Chat { get; set; }
			public string Features { get; set; }
			public string Subscriptions { get; set; }
			public string Editors { get; set; }
			public string Teams { get; set; }
			public string Videos { get; set; }
		}

		public class Channel {
			public bool Mature { get; set; }
			public string Status { get; set; }
			public string BroadcasterLanguage { get; set; }
			public string DisplayName { get; set; }
			public string Game { get; set; }
			public int Delay { get; set; }
			public string Language { get; set; }
			public int Id { get; set; }
			public string Name { get; set; }
			public string CreatedAt { get; set; }
			public string UpdatedAt { get; set; }
			public string Logo { get; set; }
			public object Banner { get; set; }
			public string VideoBanner { get; set; }
			public object Background { get; set; }
			public string ProfileBanner { get; set; }
			public object ProfileBannerBackgroundColor { get; set; }
			public bool Partner { get; set; }
			public string Url { get; set; }
			public int Views { get; set; }
			public int Followers { get; set; }
			public Links2 Links { get; set; }
		}

		public class Stream {
			public long Id { get; set; }
			public string Game { get; set; }
			public int Viewers { get; set; }
			public string CreatedAt { get; set; }
			public int VideoHeight { get; set; }
			public double AverageFps { get; set; }
			public int Delay { get; set; }
			public bool IsPlaylist { get; set; }
			public Links Links { get; set; }
			public Preview Preview { get; set; }
			public Channel Channel { get; set; }
		}

		public class Links3 {
			public string Self { get; set; }
			public string Channel { get; set; }
		}

		public class RootObject {
			public Stream Stream { get; set; }
			public Links3 Links { get; set; }
		}
	}
}
