# Ohjelmointipaja
SuperSecretGameDevelopment



## Training

1. ***Load*** the FightingPit scene
2. ***Start*** mlagents-learn
3. Press ***play*** in Unity to start the scene

***Initial***
```
mlagents-learn ChampionCommander.yaml --run-id champions1 --train --torch-device cuda
```

***Resume run id***
```
mlagents-learn ChampionCommander.yaml --run-id champions1 --train --resume --torch-device cuda
```


## Training parameters

- See [Training Configuration File](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Training-Configuration-File.md)
