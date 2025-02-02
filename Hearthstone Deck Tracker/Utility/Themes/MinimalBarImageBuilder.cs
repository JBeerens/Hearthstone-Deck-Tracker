﻿using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AForge.Imaging.Filters;
using Hearthstone_Deck_Tracker.Hearthstone;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using HearthDb.Enums;

namespace Hearthstone_Deck_Tracker.Utility.Themes
{
	public class MinimalBarImageBuilder : CardBarImageBuilder
	{
		public MinimalBarImageBuilder(Card card, string dir) : base(card, dir)
		{
			CreatedIconOffset = -15;
		}

		protected override void AddCardImage()
		{
			var cardFile = Path.Combine(BarImageDir, Card.Id + ".png");
			if(!File.Exists(cardFile))
				return;
			var img = AForge.Imaging.Image.FromFile(cardFile);
			new GaussianBlur(2, 8).ApplyInPlace(img);
			DrawingGroup.Children.Add(new ImageDrawing(BitmapToImageSource(img), FrameRect));
		}

		protected override void AddCountBox()
		{
		}

		protected override SolidColorBrush CountTextBrush
		{
			get
			{
				switch(Card.Rarity)
				{
					case Rarity.RARE:
						return new SolidColorBrush(Color.FromRgb(49, 134, 222));
					case Rarity.EPIC:
						return new SolidColorBrush(Color.FromRgb(173, 113, 247));
					case Rarity.LEGENDARY:
						return new SolidColorBrush(Color.FromRgb(255, 154, 16));
					default:
						return Brushes.White;
				}
			}
		}

		private BitmapImage BitmapToImageSource(Bitmap bitmap)
		{
			using(var memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				var bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
				return bitmapimage;
			}
		}
	}
}