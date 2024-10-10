using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public void SaveSensitvity(float _data)
    //================================================= Sensitivy 
    {
        PlayerPrefs.SetFloat("game_setting_sensitivity", _data);
    }

    public float GetSensitivity()
    {
        if (PlayerPrefs.HasKey("game_setting_sensitivity"))
            return PlayerPrefs.GetFloat("game_setting_sensitivity");
        else
            return 100f;
    }

    //=================================================

    public void SetPlayersCurrentCheckpoint(int _checkpoint)
    {
        PlayerPrefs.SetInt("game_players_checkpoint", _checkpoint);
    }

    public int GetCheckPoint()
    {
        if (PlayerPrefs.HasKey("game_players_checkpoint"))
            return PlayerPrefs.GetInt("game_players_checkpoint");
        else
            return 0;
    }

    public void SetPlayerDaggerState(bool _state)
    {
        if (_state) // if state is true set to 1 else set to 0
        {
            PlayerPrefs.SetInt("game_players_dagger_state", 1);
        }
        else
        {
            PlayerPrefs.SetInt("game_players_dagger_state", 0);
        }
    }

    public bool GetPlayerDaggerState()
    {
        if (PlayerPrefs.HasKey("game_players_dagger_state"))
        {
            int _ConvertedBool = PlayerPrefs.GetInt("game_players_dagger_state");

            if (_ConvertedBool == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    public void SetPlayersCurrentObjective(int _PlayersOjective)
    {
        PlayerPrefs.SetInt("game_players_objective", _PlayersOjective);
    }

    public int GetPlayersCurrentObjective()
    {
        if (PlayerPrefs.HasKey("game_players_objective"))
            return PlayerPrefs.GetInt("game_players_objective");
        else
            return 0;
    }

    public void SaveCurrentFloor(int _floor)
    {
        PlayerPrefs.SetInt("game_chapter_floor", _floor);
    }

    public int GetCurrentFloor()
    {
        if (PlayerPrefs.HasKey("game_chapter_floor"))
            return PlayerPrefs.GetInt("game_chapter_floor");
        else
            return 0;
    }

    public void SaveSubtitles(bool _state)
    {
        if (_state)
        {
            PlayerPrefs.SetInt("game_settings_subtitles", 1);
        }
        else
        {
            PlayerPrefs.SetInt("game_settings_subtitles", 0);
        }
    }

    public bool GetSubtitlesState()
    {
        int ConvertedInt = PlayerPrefs.GetInt("game_settings_subtitles");

        if (PlayerPrefs.HasKey("game_settings_subtitles"))
        {
            if (ConvertedInt == 0)
                return false;
            else
                return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSettings() // Delete the players settings only
    {
        PlayerPrefs.DeleteKey("game_settings_subtitles");
        PlayerPrefs.DeleteKey("game_setting_sensitivity");
    }

    public void DeleteGameProgress()
    {
        PlayerPrefs.DeleteKey("game_chapter_floor");
        PlayerPrefs.DeleteKey("game_players_objective");
        PlayerPrefs.DeleteKey("game_players_checkpoint");
        PlayerPrefs.DeleteKey("game_players_dagger_state");
    }

    public void DeleteAllData() // Delete all data in the game including settings.
    {
        PlayerPrefs.DeleteAll();
    }
}
