﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public static class Constant
    {
        public const int SPAWN_EVENT_ID = 0;
        public const int DESPAWN_EVENT_ID = 1;
        public const int Local_SPAWN_EVENT_ID = 2;
        public const int Local_DESPAWN_EVENT_ID = 3;

        public const float FLASH_OFFSET = 2.5f;
        
        public const float ACTOR_SUB_DEFAULT_LIFETIME = 1f;
        public const float ACTOR_SUB_DEFAULT_MOVE_SPEED = 10f;
        public const float AHRI_HEART_MOVE_SPEED = 8f;
        public const float AHRI_ORB_MOVE_SPEED = 10f;
        public const float ANIMALCROSSING_FISHBULLET_MOVE_SPEED = 5f;
        public const float ANIMALCROSSING_FISHBULLET_LIFETIME = 5f;
        public const float JETT_SHURIKEN_MOVE_SPEED = 20f;
        public const float NORMAL_BULLET_MOVE_SPEED = 15f;
        public const float KAKASHI_SHURIKEN_MOVE_SPEED = 15f;
        public const float NARUTO_RASENGAN_MOVE_SPEED = 3f;
        public const float MINECRAFT_SLAVE_MOVE_SPEED = 8f;
        public const float BATTLEGROUND_AIM_MOVE_SPEED = 10f;
        public const float BATTLEGROUND_THROW_MOVE_SPEED = 10f;
        public const float BATTLEGROUND_THROW_LIFETIME = 0.65f;
        public const float VENGENPRO_NOTE_MOVE_SPEED = 5f;
        public const float VENGENPRO_ZZANG_MOVE_SPEED = 5f;
        public const float TREE_LEFT_MOVE_SPEED = 1f;

        public const string PANZEE_BLUE = "Panzee_Blue";
        public const string PANZEE_RED = "Panzee_Red";
        public const string PANZEE_BLUE_TSHIRT = "Panzee_Blue_TShirt";
        public const string PANZEE_RED_TSHIRT = "Panzee_Red_TShirt";

        public static readonly Color HEART_COLOR = new Color(255f / 255f, 20f / 255f, 150f / 255f, 1f);
    }
}