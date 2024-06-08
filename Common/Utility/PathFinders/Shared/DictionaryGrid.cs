using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace androLib.Common.Utility {
	public class DictionaryGrid<T> : IEnumerable<(int x, int y, T value)> {
		private SortedDictionary<int, SortedDictionary<int, T>> Grid;
		public DictionaryGrid() {
			Grid = new();
			Count = 0;
		}
		public int Count;
		public void Add(int x, int y, T value) {
			if (!Grid.TryGetValue(x, out SortedDictionary<int, T> yDict)) {
				yDict = new();
				Grid.Add(x, yDict);
			}

			yDict.Add(y, value);
			Count++;
		}
		public void AddOrSet(int x, int y, T value) {
			if (!Grid.TryGetValue(x, out SortedDictionary<int, T> yDict)) {
				yDict = new();
				Grid.Add(x, yDict);
			}

			if (yDict.AddOrSet(y, value))
				Count++;
		}
		public bool TryAdd(int x, int y, T value) {
			if (!Grid.TryGetValue(x, out SortedDictionary<int, T> yDict)) {
				yDict = new();
				Grid.Add(x, yDict);
			}

			if (yDict.TryAdd(y, value)) {
				Count++;
				return true;
			}

			return false;
		}
		public void Set(int x, int y, T value) => Grid[x][y] = value;
		public bool TryGetValue(int x, int y, out T value) {
			if (Grid.TryGetValue(x, out SortedDictionary<int, T> yDict)) {
				if (yDict.TryGetValue(y, out value))
					return true;
			}

			value = default;
			return false;
		}
		public bool Contains(int x, int y) => Grid.TryGetValue(x, out SortedDictionary<int, T> yDict) ? yDict.ContainsKey(y) : false;

		public bool TryRemove(int x, int y) {
			if (Grid.TryGetValue(x, out SortedDictionary<int, T> yDict)) {
				if (yDict.ContainsKey(y)) {
					yDict.Remove(y);
					if (yDict.Count == 0)
						Grid.Remove(x);

					Count--;

					return true;
				}
			}

			return false;
		}
		public T[] ToArraySingleArray() { 
			T[] array = new T[Count];
			int i = 0;
			foreach (KeyValuePair<int, SortedDictionary<int, T>> p in Grid) {
				foreach (KeyValuePair<int, T> p2 in p.Value) {
					array[i] = p2.Value;
					i++;
				}
			}

			return array;
		}
		public void GetRandomAndRemove(out int x, out int y, out T value) {
			int i = Main.rand.Next(Count);
			foreach (KeyValuePair<int, SortedDictionary<int, T>> p in Grid) {
				foreach (KeyValuePair<int, T> p2 in p.Value) {
					if (i == 0) {
						x = p.Key;
						y = p2.Key;
						value = p2.Value;
						TryRemove(x, y);

						return;
					}

					i--;
				}
			}

			throw new Exception("DictionaryGrid is empty");
		}
		public IEnumerable<(int x, int y)> Keys {
			get {
				foreach (KeyValuePair<int, SortedDictionary<int, T>> p in Grid) {
					foreach (KeyValuePair<int, T> p2 in p.Value) {
						yield return (p.Key, p2.Key);
					}
				}
			}
		}
		public IEnumerable<T> Values {
			get {
				foreach (KeyValuePair<int, SortedDictionary<int, T>> p in Grid) {
					foreach (KeyValuePair<int, T> p2 in p.Value) {
						yield return p2.Value;
					}
				}
			}
		}
		public IEnumerator<(int x, int y, T value)> GetEnumerator() {
			foreach (KeyValuePair<int, SortedDictionary<int, T>> p in Grid) {
				foreach (KeyValuePair<int, T> p2 in p.Value) {
					yield return (p.Key, p2.Key, p2.Value);
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public bool IsEmpty() => Grid.Count == 0;

		public bool Any() => Grid.Count > 0;
		public void Clear() {
			Grid.Clear();
			Count = 0;
		}
	}
}
