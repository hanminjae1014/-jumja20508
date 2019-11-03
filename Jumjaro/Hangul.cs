﻿using System;

namespace Jumjaro
{
    public class Hangul
    {
        private readonly char[] _onsets =
        {
            'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ','ㅅ','ㅆ','ㅇ','ㅈ',
            'ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };

        private readonly char[] _nucleuses =
        {
            'ㅏ','ㅐ','ㅑ','ㅒ','ㅓ','ㅔ','ㅕ','ㅖ','ㅗ','ㅘ','ㅙ','ㅚ','ㅛ',
            'ㅜ','ㅝ','ㅞ','ㅟ','ㅠ','ㅡ','ㅢ','ㅣ'
        };

        private readonly char[] _codas =
        {
            '\0', 'ㄱ','ㄲ','ㄳ','ㄴ','ㄵ','ㄶ','ㄷ','ㄹ','ㄺ','ㄻ','ㄼ','ㄽ','ㄾ',
            'ㄿ','ㅀ','ㅁ','ㅂ','ㅄ','ㅅ','ㅆ','ㅇ','ㅈ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
        };

        public bool IsHangulCharacter(char ch)
        {
            return (('가' <= ch) && (ch <= '힣'));
        }

        public int FindIndexOfOnset(char ch)
        {
            return Array.FindIndex(_onsets, c => c == ch);
        }

        public int FindIndexOfNucleus(char ch)
        {
            return Array.FindIndex(_nucleuses, c => c == ch);
        }

        public int FindIndexOfCoda(char ch)
        {
            return Array.FindIndex(_codas, c => c == ch);
        }

        public bool HasCoda(char ch)
        {
            var syllables = Syllabification(ch, false, false, true);
            if (syllables == null)
            {
                return false;
            }

            return syllables[2] != '\0';
        }

        public char RemoveCoda(char ch)
        {
            var syllables = Syllabification(ch, true, true, false);
            if (syllables == null)
            {
                throw new ArgumentException($"{ch} is not a Hangul character.");
            }

            return JoinSyllables(syllables[0], syllables[1]);
        }

        public char[] Syllabification(char letter, bool onset = true, bool nucleus = true, bool coda = true)
        {
            if (!IsHangulCharacter(letter))
            {
                return null;
            }

            var syllables = new char[3];

            var offset = letter - '가';
            if (onset)
            {
                syllables[0] = _onsets[offset / (_nucleuses.Length * _codas.Length)];
            }
            if (nucleus)
            {
                syllables[1] = _nucleuses[(offset / _codas.Length) % _nucleuses.Length];
            }
            if (coda)
            {
                syllables[2] = _codas[offset % _codas.Length];
            }
            return syllables;
        }

        public char JoinSyllables(char onset, char nucleus, char coda = default)
        {
            return (char)((Array.IndexOf(_onsets, onset) * _nucleuses.Length + Array.IndexOf(_nucleuses, nucleus)) * _codas.Length + Array.IndexOf(_codas, coda) + '가');
        }
    }
}
