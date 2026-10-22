using System.IO;
using UnityEngine;

namespace AppsMinistry.Domain
{
	public class GameServicePlayer : DomainObject
	{
		public string PlayerId;

		public string Alias;

		public string DisplayName;

		public bool IsFriend;

		public bool HasProfilePhoto;

		public Texture2D ProfilePhoto;

		public string ProfilePhotoPath;

		private GameServiceScore _score;

		public GameServiceScore Score
		{
			get
			{
				if (_score == null)
				{
					_score = new GameServiceScore();
				}
				return _score;
			}
			set
			{
				_score = value;
			}
		}

		public bool hasProfilePhoto
		{
			get
			{
				if (string.IsNullOrEmpty(ProfilePhotoPath))
				{
					ProfilePhotoPath = Path.Combine(Application.persistentDataPath, string.Format("{0}.png", PlayerId).Replace(":", string.Empty));
				}
				return File.Exists(ProfilePhotoPath);
			}
		}

		public void LoadPhoto()
		{
			if (hasProfilePhoto)
			{
				byte[] array = File.ReadAllBytes(ProfilePhotoPath);
				Texture2D texture2D = new Texture2D(2, 2);
				bool flag = texture2D.LoadImage(array);
				if (!flag)
				{
					byte[] data = new byte[400]
					{
						137, 80, 78, 71, 13, 10, 26, 10, 0, 0,
						0, 13, 73, 72, 68, 82, 0, 0, 0, 64,
						0, 0, 0, 64, 8, 0, 0, 0, 0, 143,
						2, 46, 2, 0, 0, 1, 87, 73, 68, 65,
						84, 120, 1, 165, 87, 209, 173, 196, 48, 8,
						131, 129, 50, 74, 102, 201, 54, 153, 133, 69,
						188, 78, 116, 189, 143, 158, 91, 212, 232, 241,
						106, 127, 221, 41, 178, 85, 12, 36, 96, 235,
						13, 48, 231, 249, 243, 133, 64, 116, 63, 240,
						82, 0, 195, 15, 188, 20, 192, 244, 11, 240,
						63, 1, 68, 243, 59, 58, 5, 138, 65, 103,
						20, 5, 24, 116, 6, 74, 2, 190, 71, 84,
						4, 134, 239, 209, 10, 2, 240, 132, 217, 157,
						40, 8, 220, 156, 31, 72, 33, 225, 79, 1,
						220, 201, 7, 194, 47, 152, 73, 96, 231, 96,
						199, 206, 211, 157, 0, 34, 2, 7, 250, 65,
						142, 39, 79, 49, 55, 2, 249, 195, 241, 124,
						210, 22, 46, 231, 182, 229, 183, 157, 167, 191,
						80, 6, 5, 74, 124, 208, 59, 74, 45, 43,
						243, 151, 147, 53, 119, 2, 184, 58, 156, 48,
						47, 129, 131, 213, 108, 85, 254, 186, 125, 25,
						91, 218, 170, 252, 206, 15, 224, 191, 83, 160,
						192, 7, 141, 255, 130, 137, 180, 26, 127, 229,
						163, 95, 70, 172, 198, 15, 186, 150, 28, 177,
						18, 127, 229, 51, 38, 210, 74, 252, 65, 7,
						179, 9, 86, 225, 227, 161, 184, 206, 60, 90,
						129, 191, 218, 67, 115, 117, 166, 113, 219, 127,
						15, 41, 36, 130, 149, 8, 175, 33, 201, 158,
						189, 80, 230, 71, 18, 56, 239, 3, 120, 17,
						43, 97, 180, 165, 11, 232, 33, 232, 38, 234,
						105, 172, 23, 18, 15, 115, 33, 41, 165, 44,
						55, 147, 222, 206, 250, 133, 162, 95, 105, 250,
						165, 170, 95, 235, 250, 195, 162, 63, 109, 250,
						227, 170, 63, 239, 250, 128, 161, 143, 56, 4,
						226, 139, 215, 67, 150, 62, 230, 233, 131, 38,
						225, 194, 168, 43, 12, 219, 194, 184, 47, 44,
						28, 194, 202, 35, 45, 93, 250, 218, 167, 47,
						158, 250, 234, 171, 47, 223, 242, 250, 255, 1,
						26, 24, 83, 131, 193, 78, 20, 27, 0, 0,
						0, 0, 73, 69, 78, 68, 174, 66, 96, 130
					};
					texture2D.LoadImage(data);
				}
				Debug.Log("GameServicePlayer::LoadPhoto " + ProfilePhotoPath + " playerId " + PlayerId + " isExist " + File.Exists(ProfilePhotoPath) + " bytesRead " + array.LongLength + " LoadImageResult " + flag);
				ProfilePhoto = texture2D;
			}
		}

		public void SavePhoto(Texture2D profilePhoto)
		{
			ProfilePhoto = profilePhoto;
			File.WriteAllBytes(ProfilePhotoPath, profilePhoto.GetRawTextureData());
		}
	}
}
