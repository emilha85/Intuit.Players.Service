# Intuit.Players.Service

## Overview

Players service:
  Exposing rest api with 2 endpoints:
  - GetAllPlayers (using pagination with limit and offset)
  - GetPlayerById

    
Inital players data in taken from csv file.

- **DBs**
   - In memory db to simulate document db (e.g Mongo)
   - in memory Cache (**LRU**) to simulate distributed key value cache (e.g Redis).

**EnrichementEngine** - Upon request to enrich player data, we are running all enrichments (that implments **IPlayerEnrichment**).

Player data can be enriched, with 2(or more) enrichments (html scrapers in this assignment).

Since the enrichment operation is time consuming and can be dependent on external api's, , data is being enriched in 2 scenarios:
  - Upon request to GetPlayerById (if entity not in cache)
  - When we recive **PlayersDataMessage**-> if player exists in db and was updated-> we are running the **EnrichmentEngine** again and saving both in db and in cache (if exists)

## Main components:

- **Jobs**
  -  **UpdatePlayerDataJob** - Responsible to read players data from source (currently csv), calling the CsvPlayersReader.
    The Job is running in intervals( interval is taken from confifguration), currenly every 12 hours;

- **CsvPlayersReader** - Reads player data in bulks, and send a message using the bus, that new players data arrived

- **Channels** - .net implmentation for in memory bus.

- **Consumers**
  - -**PlayersDataMessageListener** - Consumer of the **PlayersDataMessage** sent by CsvPlayersReader, saves data in db.
  - -**PlayersSearchesMessageListener** - Consumer of **PlayersSearchedMessage** sent by the handler. incrmenting plyers searched data. This will be used for the  in-memeory cache cold start. 

- **PlayersHandler** - handler for the api calls;

- **PlayersDb** - simulate document db.
- **PlayersCache** - simulate redis/ dual layer cache with redis.

- **StartupWorker** - startup worker to init bus consumers+ cache warmup.
- **CacheWarmUp** - running at service startup, fetching top most searched users and init cache (cold start)
