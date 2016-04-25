

using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ClimateWord {

	public string word;
	public string meaning;

}


public class Dictionary  {

	private static Dictionary s_instance;
	private ClimateWord[] words;	

	
	public static Dictionary instance {
		
		get { return s_instance == null ? load () : s_instance;}
	
	}
	
	private Dictionary (ClimateWord[] words){
	
		this.words = words;
	
	}
	
	private static bool isWordOK(string word){
	
		if(word.Length < 1){
			return false;
		}
	
		foreach (char c in word) {
		
			if(!TextUtils.isAlpha(c)){
				return false;
			}
		
		}

		return true;
	}
	
	

	public static Dictionary load(){
	
			if(s_instance != null){
				return s_instance;
			}
	
	
		HashSet<ClimateWord> wordList = new HashSet<ClimateWord> ();
		//loading word list
		TextAsset asset = Resources.Load ("words") as TextAsset;
		TextReader src = new StringReader(asset.text);
		//reading lines
		while(src.Peek() != -1){
			string fullword = src.ReadLine();
			int equalPos = fullword.IndexOf ("=");

			string word = fullword;
			string description = "";
			if (equalPos > 0) {
				word = fullword.Substring (0, equalPos);
				word = word.Replace(" ", "");
				 description = fullword.Substring (equalPos + 1);
			}

			if(isWordOK(word)){
				//wordList.Add(word);

				ClimateWord newWord = new ClimateWord ();
				newWord.word = word;
				newWord.meaning = description.Trim();
				wordList.Add (newWord);

			}
		}
		//unloading assets
		Resources.UnloadAsset(asset);
		//setup dict
		ClimateWord[] words = new ClimateWord[wordList.Count];
		wordList.CopyTo (words);
		s_instance = new Dictionary(words);
		
		return s_instance;
	}


	public int totalWords(){

		return this.words.Length;
	}

	public ClimateWord next(int limit){
		int index = (int) (Random.value * (words.Length-1));
		return words[index];
	}


}
