# FrostTrace: Cold-Chain Telemetry System

## 1. System Architecture
The system is divided into four distinct components to handle high-volume data streaming and business logic.

* **API (Management):** Handles the CRUD operations for Batches and Delivery Runs. Controls the lifecycle (Ready, In-Transit, Completed).
* **Simulator (IoT Device):** A Console App that mimics a truck sensor. It sends temperature pings to the Ingestor.
* **Ingestor (Entry Point):** A high-speed Minimal API that receives JSON pings and immediately pushes them to RabbitMQ.
* **Worker (The Brain):** A background service that processes the telemetry, tracks heat streaks, and triggers Quarantines.

---

## 2. The Data Flow

### Step 1: Batch Creation & Splitting
1.  **API** creates a `MasterBatch` (e.g., 1000 Vaccines).
2.  **API** splits the `MasterBatch` into multiple `DeliveryRuns` (e.g., Truck A, Truck B).
3.  Each `DeliveryRun` is assigned a unique `SensorID` and a `Status` of `READY`.

### Step 2: The Dispatch (Control Flow)
1.  **API** triggers a `DeliveryRun` to start.
2.  **Status** changes to `IN_TRANSIT`.
3.  **API** sends a command to the **Simulator** (via RabbitMQ or Direct Call) to start sending data for that specific `SensorID`.

### Step 3: Telemetry Intake (Data Flow)
1.  **Simulator** loops every 5 seconds, sending a JSON ping:
    ```json
    { "sensorId": "SN-001", "temp": 22.5, "timestamp": "..." }
    ```
2.  **Ingestor** receives the POST request. It checks if the `sensorId` is active.
3.  **Ingestor** pushes the message to the `telemetry_queue` in **RabbitMQ** and returns `202 Accepted`.

### Step 4: Stream Processing (The Worker)
1.  **Worker** pulls the ping from RabbitMQ.
2.  **Worker** retrieves the current "Heat Streak" for that `DeliveryRun` from **MongoDB**.
3.  **The Logic ($T$ vs $N$):**
    * If $T >$ Threshold: Increment Streak.
    * If $T \le$ Threshold: Reset Streak to 0.
4.  **The Trigger:** If Streak $== N$, the Worker updates the `DeliveryRun` status to `QUARANTINED` in MongoDB immediately.

---

## 3. Technology Stack (Local Docker Setup)
* **Database:** MongoDB (Stores Batch data and live Streak state).
* **Message Broker:** RabbitMQ (Decouples Ingestor from Worker).
* **Backend:** .NET (API, Ingestor, Worker, and Simulator).
* **Infrastructure:** Docker Compose (Orchestrates all services locally).

---

## 4. Key Learning Goals
* **Asynchronous Processing:** Moving data through a queue instead of direct DB writes.
* **Stateful Streaming:** Tracking consecutive events ($N$ pings) over time.
* **System Decoupling:** Ensuring the API stays online even if the Simulator or Worker crashes.