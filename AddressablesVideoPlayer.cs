// Copyright 2019 The Gamedev Guru (http://thegamedev.guru)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

namespace TheGamedevGuru
{
[RequireComponent(typeof(VideoPlayer))]
public class AddressablesVideoPlayer : MonoBehaviour
{
    [SerializeField] private AssetReference[] videoClipRefs = null;
    private VideoPlayer videoPlayer;
    
    public void PlayVideo(int index)
    {
        StartCoroutine(PlayVideoInternal(index));
    }

    private IEnumerator PlayVideoInternal(int index)
    {
        var asyncOperationHandle = videoClipRefs[index].LoadAssetAsync<VideoClip>();
        yield return asyncOperationHandle;
        videoPlayer.clip = asyncOperationHandle.Result;
        videoPlayer.Play();
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => videoPlayer.isPlaying == false);
        videoPlayer.clip = null;
        Addressables.Release(asyncOperationHandle);
    }

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        PlayVideo(1);
    }
}
}