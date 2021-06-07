use serde_derive::{Serialize, Deserialize};
#[derive(Serialize, Deserialize)]
pub struct Tokens {
    pub discord: String,
    pub imgur: String,
    #[serde(rename = "search-cx")]
    pub search_cx: String,
    #[serde(rename = "search-key")]
    pub search_key: String,
    #[serde(rename = "app-id")]
    pub app_id: u64,
}

#[derive(Serialize, Deserialize)]
pub struct Sites {
    #[serde(rename = "top-gg")]
    pub top_gg: Option<String>,
    #[serde(rename = "")]
    pub discord_bots: Option<String>,
    #[serde(rename = "search-key")]
    pub bots_on_discord: Option<String>,
    #[serde(rename = "search-key")]
    pub discord_bot_list: Option<String>,
}

#[derive(Serialize, Deserialize)]
pub struct Paths {
    pub unqlite: String
}

#[derive(Serialize, Deserialize)]
pub struct Config {
    pub tokens: Tokens,
    pub sites: Sites,
    pub paths: Paths,
}