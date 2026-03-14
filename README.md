# ❄️ FrostTrace

**Status:** Initial Specification  
**Focus:** Cold-Chain Integrity & Recursive Lineage

---

## 1. The Core Vision
FrostTrace is an **immutable lineage engine** for high-stakes logistics. It solves the "Food Poisoning" problem by ensuring that every movement of a product is tracked via a **Handshake Protocol** and every temperature deviation triggers a **Recursive Quarantine**.

## 2. Key System Invariants (The "Rules")
* **The Chain of Custody:** A `Batch` cannot change its `CurrentLocation` without a `TransferEvent`. This requires a digital signature (Token/QR) from both the Sender and the Receiver.
* **Immutable Lineage:** A `Batch` can be split into `ChildBatches`. Children inherit all historical telemetry data from the Parent. If a Parent is flagged, the entire branch is locked.
* **Event-Driven Quarantine:** The system does not "check" status once a day. It reacts to a **Telemetry Stream**. If $T > Threshold$ for $N$ consecutive pings, the Batch state is force-changed to `QUARANTINED`.

## 3. High-Level Architecture (Dockerized)
* **`frosttrace-api` (.NET 9):** The source of truth for Batch state and Handshakes.
* **`frosttrace-ingestor`:** A "Fire-and-Forget" service that collects JSON pings from IoT simulators.
* **`frosttrace-worker`:** Listens to the **RabbitMQ** queue to process telemetry and manage Sagas.
* **`frosttrace-db` (MongoDB):**
    * `Batches`: Relational structure for lineage.
    * `Telemetry`: Time-series collection for sensor pings.
    * `AuditLog`: Append-only collection for every state change.

## 4. Phase 1: Minimum Viable Logic
1.  **Genesis:** Create a Batch with a defined "Safe Temperature Range."
2.  **Handshake:** Implement the `InitiateTransfer` and `AcceptTransfer` endpoints.
3.  **The Breach:** Create a script that sends 10 pings. On the 5th ping, the temperature spikes. The system must automatically transition the Batch to `QUARANTINED`.

## 5. Agent Workflow Instructions
* **Architect Agent:** Maintain the API contracts and ensure no "CRUD-only" logic creeps in.
* **Developer Agent:** Focus on building the **Recursive Ancestry Lookup**.
* **QA Agent:** Simulate "Chaos" by injecting high temperatures into the `ingestor` service.