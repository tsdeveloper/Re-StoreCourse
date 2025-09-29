using Newtonsoft.Json;

namespace API.DTOs;

public class StripeChargerPayment
{
    public string id { get; set; }
    public string api_version { get; set; }
    public int created { get; set; }
    [JsonProperty("data")]
    public Data data { get; set; }
    public bool livemode { get; set; }
    public int pending_webhooks { get; set; }
    public Request request { get; set; }
    public string type { get; set; }
}

public class Data
{
    [JsonProperty("object")]
    public DetailPayment DetailPayment { get; set; }
    public Previous_attributes previous_attributes { get; set; }
}

public class DetailPayment
{
    public string id { get; set; }
    public int amount { get; set; }
    public int amount_captured { get; set; }
    public int amount_refunded { get; set; }
    public object application { get; set; }
    public object application_fee { get; set; }
    public object application_fee_amount { get; set; }
    public string balance_transaction { get; set; }
    public Billing_details billing_details { get; set; }
    public string calculated_statement_descriptor { get; set; }
    public bool captured { get; set; }
    public int created { get; set; }
    public string currency { get; set; }
    public object customer { get; set; }
    public object description { get; set; }
    public object destination { get; set; }
    public object dispute { get; set; }
    public bool disputed { get; set; }
    public object failure_balance_transaction { get; set; }
    public object failure_code { get; set; }
    public object failure_message { get; set; }
    public Fraud_details fraud_details { get; set; }
    public bool livemode { get; set; }
    public Metadata metadata { get; set; }
    public object on_behalf_of { get; set; }
    public object order { get; set; }
    public Outcome outcome { get; set; }
    public bool paid { get; set; }
    public string payment_intent { get; set; }
    public string payment_method { get; set; }
    public Payment_method_details payment_method_details { get; set; }
    public Radar_options radar_options { get; set; }
    public object receipt_email { get; set; }
    public object receipt_number { get; set; }
    public string receipt_url { get; set; }
    public bool refunded { get; set; }
    public object review { get; set; }
    public object shipping { get; set; }
    public object source { get; set; }
    public object source_transfer { get; set; }
    public object statement_descriptor { get; set; }
    public object statement_descriptor_suffix { get; set; }
    public string status { get; set; }
    public object transfer_data { get; set; }
    public object transfer_group { get; set; }
}

public class Billing_details
{
    public AddressDetails address { get; set; }
    public object email { get; set; }
    public string name { get; set; }
    public object phone { get; set; }
    public object tax_id { get; set; }
}

public class AddressDetails
{
    public object city { get; set; }
    public object country { get; set; }
    public object line1 { get; set; }
    public object line2 { get; set; }
    public string postal_code { get; set; }
    public object state { get; set; }
}

public class Fraud_details
{

}

public class Metadata
{

}

public class Outcome
{
    public object advice_code { get; set; }
    public object network_advice_code { get; set; }
    public object network_decline_code { get; set; }
    public string network_status { get; set; }
    public object reason { get; set; }
    public string risk_level { get; set; }
    public int risk_score { get; set; }
    public string seller_message { get; set; }
    public string type { get; set; }
}

public class Payment_method_details
{
    public Card card { get; set; }
    public string type { get; set; }
}

public class Card
{
    public int amount_authorized { get; set; }
    public string authorization_code { get; set; }
    public string brand { get; set; }
    public Checks checks { get; set; }
    public string country { get; set; }
    public int exp_month { get; set; }
    public int exp_year { get; set; }
    public Extended_authorization extended_authorization { get; set; }
    public string fingerprint { get; set; }
    public string funding { get; set; }
    public Incremental_authorization incremental_authorization { get; set; }
    public object installments { get; set; }
    public string last4 { get; set; }
    public object mandate { get; set; }
    public Multicapture multicapture { get; set; }
    public string network { get; set; }
    public Network_token network_token { get; set; }
    public string network_transaction_id { get; set; }
    public Overcapture overcapture { get; set; }
    public string regulated_status { get; set; }
    public object three_d_secure { get; set; }
    public object wallet { get; set; }
}

public class Checks
{
    public object address_line1_check { get; set; }
    public string address_postal_code_check { get; set; }
    public string cvc_check { get; set; }
}

public class Extended_authorization
{
    public string status { get; set; }
}

public class Incremental_authorization
{
    public string status { get; set; }
}

public class Multicapture
{
    public string status { get; set; }
}

public class Network_token
{
    public bool used { get; set; }
}

public class Overcapture
{
    public int maximum_amount_capturable { get; set; }
    public string status { get; set; }
}

public class Radar_options
{

}

public class Previous_attributes
{
    public object balance_transaction { get; set; }
    public string receipt_url { get; set; }
}

public class Request
{
    public object id { get; set; }
    public object idempotency_key { get; set; }
}
