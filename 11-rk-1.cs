using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameCore : MonoBehaviour
{
    // 资源管理
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    // 天气系统
    private string currentWeather;
    private string[] weatherTypes = { "Sunny", "Rainy", "Stormy", "Foggy" };

    // NPC 互动
    private Dictionary<string, string[]> npcDialogues = new Dictionary<string, string[]>();

    // 音效和音乐
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip[] weatherSounds;
    public AudioClip[] puzzleSounds;

    void Start()
    {
        InitResources();
        InitNPCDialogues();
        StartCoroutine(WeatherCycle());
        PlayBackgroundMusic();
    }

    // 资源管理逻辑
    private void InitResources()
    {
        resources["Wood"] = 0;
        resources["Stone"] = 0;
        resources["Food"] = 0;
    }

    public void CollectResource(string type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }
    }

    public bool UseResource(string type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            return true;
        }
        return false;
    }

    // 天气系统逻辑
    private IEnumerator WeatherCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(20, 60));
            ChangeWeather();
        }
    }

    private void ChangeWeather()
    {
        currentWeather = weatherTypes[UnityEngine.Random.Range(0, weatherTypes.Length)];
        Debug.Log("Weather changed to: " + currentWeather);
        PlayWeatherSound();
    }

    private void PlayWeatherSound()
    {
        int index = Array.IndexOf(weatherTypes, currentWeather);
        if (index >= 0 && index < weatherSounds.Length)
        {
            sfxSource.PlayOneShot(weatherSounds[index]);
        }
    }

    // 解谜系统
    public bool SolvePuzzle(int puzzleID, string answer)
    {
        Dictionary<int, string> puzzleAnswers = new Dictionary<int, string>()
        {
            { 1, "sun" },
            { 2, "stone" },
            { 3, "water" }
        };
        
        if (puzzleAnswers.ContainsKey(puzzleID) && puzzleAnswers[puzzleID] == answer.ToLower())
        {
            sfxSource.PlayOneShot(puzzleSounds[UnityEngine.Random.Range(0, puzzleSounds.Length)]);
            return true;
        }
        return false;
    }

    // NPC 互动逻辑
    private void InitNPCDialogues()
    {
        npcDialogues["Old Pirate"] = new string[] {
            "Ahoy! Ye be needin' help?",
            "Find the hidden treasure and beware the storm!"
        };
    }

    public string GetNPCDialogue(string npcName)
    {
        if (npcDialogues.ContainsKey(npcName))
        {
            return npcDialogues[npcName][UnityEngine.Random.Range(0, npcDialogues[npcName].Length)];
        }
        return "...";
    }

    // 多结局逻辑
    public string GetGameEnding(int playerChoice)
    {
        switch (playerChoice)
        {
            case 1: return "You escaped the island successfully!";
            case 2: return "You became the island's new ruler.";
            case 3: return "You perished in the storm...";
            default: return "Your fate remains unknown.";
        }
    }

    // 音效与音乐
    private void PlayBackgroundMusic()
    {
        if (bgmSource && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }
}