# ❄️ FrostTrace: High-Integrity Cold-Chain Engine

**Architecture:** Distributed Microservices  
**Stack:** .NET 10, MongoDB, RabbitMQ, Docker  
**Domain:** Logistics & Supply Chain Integrity  

---

## 1. Business Core Logic (The Problem)
In high-stakes logistics (Pharmaceuticals, Premium Seafood), "Food Poisoning" or product loss occurs because of two failures:
1. **Lack of Custody Proof:** No one knows exactly when the temperature spiked or who was responsible.
2. **Delayed Reaction:** Spikes are discovered at the destination, after the product is already ruined.

**FrostTrace** solves this by automating the "Quarantine" the moment a thermal breach occurs and enforcing a digital "Handshake" between actors.

---

## 2. Service Architecture
This project is split into three decoupled services to ensure high availability and scalability.

### A. `FrostTrace.Api` (The Command Center)
* **Responsibility:** Manages the source of truth for Batches and Handshakes.
* **Core Flow:** * Creates "Genesis" batches with defined thermal thresholds.
    * Generates Handshake Tokens (UUID/QR) for transfers.
    * Updates ownership once both parties verify the handshake.
* **Database:** `Batches` collection (Parent-Child relationships).

### B. `FrostTrace.Ingestor` (The Data Sink)
* **Responsibility:** High-speed intake of IoT sensor data.
* **Core Flow:** * Receives JSON pings: `{ batchId, temp, timestamp, gps }`.
    * Performs **zero** business logic to remain fast.
    * Pushes every ping onto a **RabbitMQ** exchange (Routing Key: `telemetry.raw`).

### C. `FrostTrace.Worker` (The Brain)
* **Responsibility:** Processes telemetry and enforces business rules (Sagas).
* **Core Flow:** * Consumes from RabbitMQ.
    * **Threshold Check:** Compares pings against the Batch's safe range.
    * **Auto-Quarantine:** If 3 consecutive pings exceed the limit, it emits a `ThermalBreach` event.
    * **Lineage Locking:** Traces down the tree to flag all sub-batches as `COMPROMISED`.

---

## 3. The "Frozen Throne" Business Flow


1. **Initialization:** Producer creates a Batch (e.g., "Vaccine-Batch-001", Temp: 2°C - 8°C).
2. **Custody Transfer:** - Shipper (A) initiates transfer to Driver (B).
   - Batch status becomes `IN_TRANSIT`.
3. **Active Monitoring:** - IoT Simulator sends pings to the **Ingestor**.
   - **Worker** monitors the pings.
4. **The Breach (The "Meat" of the project):** - If Temp hits 12°C, the **Worker** triggers a status update to `QUARANTINED`.
   - The **API** prevents Driver (B) from completing the "Acceptance" handshake.
5. **Traceability:** Consumer scans a QR code; the API performs a **Recursive Lookup** to show every temperature ping and every person who touched the product.
