﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using dnSpy.Contracts.Text.Classification;
using Microsoft.VisualStudio.Text.Classification;

namespace dnSpy.Debugger.UI.Wpf {
	/// <summary>
	/// Contains enough information to create a new text block to show in the UI. A new text block
	/// isn't created if the new <see cref="TextBlockContentInfo"/> is identical to the old instance.
	/// </summary>
	sealed class TextBlockContentInfo : IEquatable<TextBlockContentInfo> {
		public ITextElementFactory TextElementFactory { get; }
		/// <summary>
		/// Version number. It's incremented when the theme has changed to make sure the UI element is regenerated.
		/// </summary>
		public int Version { get; }
		public IClassificationFormatMap ClassificationFormatMap { get; }
		public string Text { get; }
		public TextClassificationTag[] Tags { get; }
		public TextElementFlags TextElementFlags { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="textElementFactory"></param>
		/// <param name="version">Version number. It's incremented when the theme has changed to make sure the UI element is regenerated.</param>
		/// <param name="classificationFormatMap"></param>
		/// <param name="text"></param>
		/// <param name="tags"></param>
		/// <param name="textElementFlags"></param>
		public TextBlockContentInfo(ITextElementFactory textElementFactory, int version, IClassificationFormatMap classificationFormatMap, string text, TextClassificationTag[] tags, TextElementFlags textElementFlags) {
			TextElementFactory = textElementFactory ?? throw new ArgumentNullException(nameof(textElementFactory));
			Version = version;
			ClassificationFormatMap = classificationFormatMap ?? throw new ArgumentNullException(nameof(classificationFormatMap));
			Text = text ?? throw new ArgumentNullException(nameof(text));
			Tags = tags ?? throw new ArgumentNullException(nameof(tags));
			TextElementFlags = textElementFlags;
		}

		public static bool operator !=(TextBlockContentInfo left, TextBlockContentInfo right) => !(left == right);
		public static bool operator ==(TextBlockContentInfo left, TextBlockContentInfo right) {
			if ((object)left == right)
				return true;
			if ((object)left == null)
				return false;
			return left.Equals(right);
		}

		public bool Equals(TextBlockContentInfo other) =>
			other != null &&
			TextElementFactory == other.TextElementFactory &&
			Version == other.Version &&
			ClassificationFormatMap == other.ClassificationFormatMap &&
			Text == other.Text &&
			CompareTags(Tags, other.Tags) &&
			TextElementFlags == other.TextElementFlags;

		static bool CompareTags(TextClassificationTag[] a, TextClassificationTag[] b) {
			if (a == b)
				return true;
			if (a == null || b == null)
				return false;
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++) {
				var ai = a[i];
				var bi = b[i];
				if (ai.Span != bi.Span || ai.ClassificationType != bi.ClassificationType)
					return false;
			}
			return true;
		}

		public override bool Equals(object obj) => Equals(obj as TextBlockContentInfo);

		public override int GetHashCode() {
			int hc = 0;
			hc ^= TextElementFactory.GetHashCode();
			hc ^= Version;
			hc ^= ClassificationFormatMap.GetHashCode();
			hc ^= Text.GetHashCode();
			hc ^= Tags.Length == 0 ? 0 : Tags[0].Span.GetHashCode() ^ Tags[0].ClassificationType.GetHashCode();
			hc ^= (int)TextElementFlags;
			return hc;
		}
	}
}