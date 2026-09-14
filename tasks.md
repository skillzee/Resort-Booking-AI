# Online Resort Booking System --- Project Task Tracker

> **Status:** Active\
> **Technology:** ASP.NET Core 10 Web API + ASP.NET Core 10 MVC + Azure
> OpenAI + Semantic Kernel\
> **Architecture:** Clean Architecture, no persistent database,
> in-memory runtime state\
> **Source of requirements:** Project SRS supplied with the project

------------------------------------------------------------------------

## 0. Project Goal

Build an AI-native Online Resort Booking System that can:

-   Understand natural-language resort requests.
-   Search resort and room information.
-   Check room availability.
-   Calculate pricing.
-   Answer resort-policy questions using native RAG.
-   Use Semantic Kernel plugins/tools.
-   Coordinate specialized AI agents through a Master Orchestrator.
-   Hold/reserve inventory and create booking vouchers.
-   Stream agent/tool execution back to the MVC UI.
-   Provide a conversational booking experience instead of relying only
    on traditional filters.

The SRS explicitly requires runtime/in-memory data rather than a
persistent database, native RAG with in-memory vector collections,
Semantic Kernel, specialized agents, and an ASP.NET Core MVC frontend.

------------------------------------------------------------------------

# 1. Solution & Clean Architecture

## 1.1 Solution setup

-   [x] Create solution `OnlineResortBooking`
-   [x] Create `OnlineResortBooking.Api`
-   [x] Create `OnlineResortBooking.Application`
-   [x] Create `OnlineResortBooking.Domain`
-   [x] Create `OnlineResortBooking.Infrastructure`
-   [x] Add projects to solution
-   [x] Configure project references
-   [x] Verify dependency direction
-   [x] Confirm initial solution builds

### Dependency direction

``` text
Api
 ├── Application
 └── Infrastructure

Infrastructure
 ├── Application
 └── Domain

Application
 └── Domain

Domain
 └── No infrastructure/framework dependency
```

------------------------------------------------------------------------

# 2. Domain Layer

The SRS identifies Resort, RoomType, BookingReservation, GuestProfile
and PricingRule as core domain objects, together with DateRange,
MonetaryAmount and AmenitySpec value objects.

## 2.1 Entities

-   [x] Create `Resort`
    -   [x] Id
    -   [x] Name
    -   [x] Location
    -   [x] Description
    -   [x] Amenities
    -   [x] RoomTypes
    -   [x] AddRoomType()
-   [x] Create `RoomType`
    -   [x] Id
    -   [x] Name
    -   [x] PricePerNight
    -   [x] HasPrivatePool
    -   [x] Description
    -   [x] Amenities
-   [x] Create `GuestProfile`
    -   [x] Id
    -   [x] FirstName
    -   [x] LastName
    -   [x] Email
    -   [x] NumberOfGuests
-   [x] Create `BookingReservation`
    -   [x] Id
    -   [x] ResortId
    -   [x] RoomTypeId
    -   [x] Guest
    -   [x] Stay
    -   [x] TotalAmount
    -   [x] CreatedAt
-   [ ] Create `PricingRule`
    -   [ ] Seasonal pricing support
    -   [ ] Rule type/model
    -   [ ] Effective date range
    -   [ ] Rate adjustment
    -   [ ] Domain validation

## 2.2 Value Objects

-   [x] Create `DateRange`
    -   [x] Check-in validation
    -   [x] Check-out validation
    -   [x] Nights calculation
-   [x] Create `MonetaryAmount`
    -   [x] Amount validation
    -   [x] Currency
-   [x] Create `AmenitySpec`

## 2.3 Agent/domain protocols

The SRS also calls for abstractions for multi-agent messages,
tool-execution contexts and chat-history sessions.

-   [ ] Define agent message abstraction
-   [ ] Define agent/tool execution context
-   [ ] Define chat session/history abstraction
-   [ ] Keep these abstractions independent from Semantic Kernel

------------------------------------------------------------------------

# 3. Resort Repository & Runtime Resort Data

## 3.1 Repository

-   [x] Create `IResortRepository`
-   [x] Create `InMemoryResortRepository`
-   [x] Implement `GetAll()`
-   [x] Implement `GetById()`
-   [x] Implement location search
-   [x] Implement room-type search
-   [x] Implement private-pool filtering

## 3.2 Seed data

-   [x] Seed Azure Lagoon Ocean Resort
-   [x] Seed Royal Beachfront Pool Villa
-   [x] Seed Ocean View Suite
-   [x] Seed Garden Villa
-   [x] Add resort amenities
-   [x] Add room amenities
-   [x] Register repository through dependency injection

## 3.3 Resort API

-   [x] `GET /api/v1/resorts`
-   [x] `GET /api/v1/resorts/{id}`
-   [x] `GET /api/v1/resorts/search`
-   [x] Verify Swagger integration

------------------------------------------------------------------------

# 4. Inventory & Availability

The SRS requires in-memory inventory state backed by thread-safe
collections such as `ConcurrentDictionary`.

## 4.1 Inventory model

-   [x] Create `InventoryItem`
-   [x] Define RoomType → inventory relationship
-   [x] Create `IInventoryService`

## 4.2 In-memory inventory

-   [x] Create `InMemoryInventoryService`
-   [x] Use `ConcurrentDictionary`
-   [x] Store reservations by room type
-   [x] Implement date-overlap detection
-   [x] Implement `IsAvailable()`
-   [x] Implement `Reserve()`
-   [x] Implement `Release()`
-   [x] Add thread-safety around reservation lists

## 4.3 Inventory seeding

-   [x] Seed villa inventory
-   [x] Seed suite inventory
-   [x] Seed garden-villa inventory
-   [x] Use the shared `IResortRepository`
-   [x] Register inventory service as singleton

## 4.4 Availability API

-   [x] Create inventory controller
-   [x] Create availability endpoint
-   [x] Parse `DateOnly`
-   [x] Validate `DateRange`
-   [ ] Fix/verify current availability result returning `false`
-   [ ] Verify a fresh room returns `true`
-   [ ] Verify reservation reduces availability
-   [ ] Verify overlapping reservations
-   [ ] Verify non-overlapping reservations
-   [ ] Verify inventory cannot be overbooked
-   [ ] Verify release restores availability

## 4.5 Inventory locking

The final booking workflow needs a reliable reservation/locking
operation.

-   [ ] Introduce room/inventory lock result
-   [ ] Prevent race-condition overbooking
-   [ ] Return useful failure information
-   [ ] Add lock/hold expiration if required by final workflow
-   [ ] Add booking voucher association
-   [ ] Test concurrent reservation attempts

------------------------------------------------------------------------

# 5. Pricing Engine

The SRS requires dynamic rates and a PricingPlugin that calculates
totals.

## 5.1 Basic pricing

-   [x] Create `IPricingService`
-   [x] Create `PricingService`
-   [x] Calculate `PricePerNight × Nights`
-   [x] Return `MonetaryAmount`
-   [x] Register pricing service

## 5.2 Dynamic pricing

-   [ ] Implement `PricingRule`
-   [ ] Add seasonal rates
-   [ ] Add effective date ranges
-   [ ] Support rate adjustments
-   [ ] Calculate dynamic nightly price
-   [ ] Calculate subtotal
-   [ ] Add taxes/fees if required by the booking scenario
-   [ ] Return a structured pricing breakdown

## 5.3 Pricing tool

-   [ ] Create `PricingPlugin`
-   [ ] Expose `CalculateTotal`
-   [ ] Add Semantic Kernel `[KernelFunction]`
-   [ ] Add clear tool description
-   [ ] Add parameter descriptions
-   [ ] Ensure AI cannot invent prices
-   [ ] Test pricing tool independently

------------------------------------------------------------------------

# 6. Booking Application Workflow

This is the deterministic business workflow that the AI agents will
eventually call.

## 6.1 Booking contracts

-   [ ] Create booking request DTO
-   [ ] Create guest request DTO
-   [ ] Create booking result DTO
-   [ ] Create booking confirmation/voucher DTO
-   [ ] Create structured booking failure result

## 6.2 Booking service

Create a service/use-case responsible for:

``` text
Booking Request
      ↓
Validate
      ↓
Find Resort/Room
      ↓
Validate Guests
      ↓
Check Availability
      ↓
Calculate Price
      ↓
Reserve Inventory
      ↓
Create BookingReservation
      ↓
Create Voucher
```

Tasks:

-   [ ] Create booking application service
-   [ ] Validate room type
-   [ ] Validate resort
-   [ ] Validate guest count
-   [ ] Validate dates
-   [ ] Check availability
-   [ ] Calculate price
-   [ ] Reserve inventory
-   [ ] Create `BookingReservation`
-   [ ] Generate booking confirmation number
-   [ ] Store booking voucher in memory
-   [ ] Return structured confirmation

## 6.3 Booking API

-   [ ] Create `POST /api/v1/booking/confirm`
-   [ ] Validate request
-   [ ] Call application service
-   [ ] Return confirmation payload
-   [ ] Return appropriate error responses
-   [ ] Verify booking through Swagger

------------------------------------------------------------------------

# 7. Native Tool Layer

The SRS calls for native C# methods/tool adapters for pricing, room
locking and notification mocking.

## 7.1 Inventory plugin

-   [ ] Create `InventoryPlugin`
-   [ ] `SearchVillas`
-   [ ] `CheckAvailability`
-   [ ] `Reserve/LockRoom`
-   [ ] `ReleaseRoom`
-   [ ] Return structured JSON-friendly results
-   [ ] Add `[KernelFunction]`
-   [ ] Add descriptions
-   [ ] Add parameter metadata

Expected search parameters include:

``` text
location
roomType
hasPrivatePool
checkIn
nights
maxPricePerNight
guestCount
```

## 7.2 Pricing plugin

-   [ ] Create `PricingPlugin`
-   [ ] `CalculateTotal`
-   [ ] Dynamic pricing
-   [ ] Taxes/fees
-   [ ] Structured result
-   [ ] `[KernelFunction]`

## 7.3 Notification adapter

-   [ ] Create notification abstraction
-   [ ] Create mock notification service
-   [ ] Generate booking notification
-   [ ] Log/mock email or SMS dispatch
-   [ ] Keep external integration replaceable

------------------------------------------------------------------------

# 8. Native RAG Engine

The SRS specifically requires native RAG using chunking, embedding
generation and in-memory vector collections.

## 8.1 RAG architecture

``` text
PDF/Documents
     ↓
Text Extraction
     ↓
Chunking
     ↓
Embedding Generation
     ↓
In-memory Vector Collection
     ↓
Cosine Similarity Search
     ↓
Top-K Relevant Chunks
     ↓
LLM Answer
```

## 8.2 Document ingestion

-   [ ] Create document model
-   [ ] Create document chunk model
-   [ ] Load resort policy PDFs
-   [ ] Extract PDF text
-   [ ] Preserve document/source metadata
-   [ ] Implement chunking
-   [ ] Configure chunk size
-   [ ] Configure overlap
-   [ ] Create stable chunk IDs

## 8.3 Embeddings

-   [ ] Configure Azure OpenAI embedding deployment
-   [ ] Generate embeddings
-   [ ] Store embeddings in memory
-   [ ] Store source document/chunk metadata
-   [ ] Handle embedding failures

The SRS example references `text-embedding-3`.

## 8.4 Vector search

-   [ ] Implement cosine similarity
-   [ ] Normalize vectors where appropriate
-   [ ] Search top-K chunks
-   [ ] Apply relevance threshold
-   [ ] Return source metadata
-   [ ] Return similarity score
-   [ ] Test similarity calculations

## 8.5 RAG service

-   [ ] Create `IRagService`
-   [ ] Create native RAG implementation
-   [ ] Implement ingestion
-   [ ] Implement search
-   [ ] Implement context building
-   [ ] Prevent unsupported answers when no relevant chunk is found
-   [ ] Include source grounding information

## 8.6 RAG API/testing endpoint

-   [ ] Add development RAG search endpoint if useful
-   [ ] Upload/load sample policy documents
-   [ ] Test late check-in query
-   [ ] Test pet policy query
-   [ ] Test cancellation policy query
-   [ ] Test dining policy query
-   [ ] Test irrelevant query
-   [ ] Verify top-K results

------------------------------------------------------------------------

# 9. Azure OpenAI + Semantic Kernel Foundation

Use Azure OpenAI for the AI layer.

## 9.1 Configuration

-   [ ] Add Azure OpenAI endpoint configuration
-   [ ] Add chat deployment configuration
-   [ ] Add embedding deployment configuration
-   [ ] Keep secrets out of source control
-   [ ] Use configuration/options abstraction
-   [ ] Verify Azure OpenAI connection

## 9.2 Semantic Kernel

-   [ ] Add Semantic Kernel packages
-   [ ] Create Kernel configuration
-   [ ] Register Azure OpenAI chat completion
-   [ ] Register embedding service
-   [ ] Configure dependency injection
-   [ ] Verify simple SK prompt execution

## 9.3 Tool calling

-   [ ] Register InventoryPlugin
-   [ ] Register PricingPlugin
-   [ ] Register RAG search tool
-   [ ] Register BookingPlugin
-   [ ] Verify model can select appropriate tools
-   [ ] Inspect tool-call results
-   [ ] Handle malformed tool arguments
-   [ ] Handle tool errors

------------------------------------------------------------------------

# 10. Semantic Kernel Plugins

The SRS requires Semantic Kernel plugin definitions in the Application
layer.

## 10.1 Search/RAG plugin

-   [ ] Create `SearchPlugin`
-   [ ] Search resort information
-   [ ] Search RAG knowledge
-   [ ] Return structured search results
-   [ ] Add descriptions

## 10.2 Booking & Inventory plugin

-   [ ] Create `InventoryPlugin`
-   [ ] Search villas
-   [ ] Check availability
-   [ ] Calculate/return inventory state
-   [ ] Lock/reserve room
-   [ ] Release reservation
-   [ ] Add tool descriptions

## 10.3 Pricing plugin

-   [ ] Create `PricingPlugin`
-   [ ] Calculate dynamic price
-   [ ] Return pricing breakdown

## 10.4 Policy plugin

-   [ ] Create `PolicyPlugin`
-   [ ] Search policies through RAG
-   [ ] Return grounded chunks
-   [ ] Do not answer from invented policy data

## 10.5 Booking plugin

-   [ ] Create `BookingPlugin`
-   [ ] Execute reservation
-   [ ] Generate voucher
-   [ ] Return confirmation payload

------------------------------------------------------------------------

# 11. Conversation / Chat Session

The SRS expects conversational state and thread-safe session memory.

## 11.1 Chat models

-   [ ] Create chat request
-   [ ] Create chat response
-   [ ] Create conversation/session model
-   [ ] Create agent message model
-   [ ] Track user messages
-   [ ] Track assistant messages
-   [ ] Track tool execution messages

## 11.2 Session memory

-   [ ] Implement in-memory chat session store
-   [ ] Use thread-safe collections
-   [ ] Support session ID
-   [ ] Preserve relevant booking context
-   [ ] Prevent unrelated session leakage
-   [ ] Add cleanup/expiration strategy

------------------------------------------------------------------------

# 12. Specialized AI Agents

The SRS defines three worker agents.

## 12.1 Search & RAG Agent

Responsibilities:

-   [ ] Receive knowledge-search task
-   [ ] Search vector index
-   [ ] Retrieve relevant document chunks
-   [ ] Return grounded context
-   [ ] Provide source information
-   [ ] Avoid unsupported policy claims

## 12.2 Booking & Inventory Agent

Responsibilities:

-   [ ] Search room inventory
-   [ ] Check availability
-   [ ] Apply pricing tools
-   [ ] Respect price ceiling
-   [ ] Respect guest count
-   [ ] Lock/reserve inventory
-   [ ] Execute booking tools only after confirmation

## 12.3 Policy & Concierge Agent

Responsibilities:

-   [ ] Receive policy questions
-   [ ] Search policy RAG
-   [ ] Answer cancellation policy
-   [ ] Answer pet policy
-   [ ] Answer dining policy
-   [ ] Answer late check-in policy
-   [ ] Answer seasonal activity questions
-   [ ] Ground answers in retrieved documents

------------------------------------------------------------------------

# 13. Master Orchestrator

This is the central AI agent responsible for intent recognition, routing
and synthesis.

The SRS expects the Master Orchestrator to decompose requests into tasks
and delegate them to worker agents.

## 13.1 Intent analysis

-   [ ] Create orchestrator abstraction
-   [ ] Detect search intent
-   [ ] Detect booking intent
-   [ ] Detect policy intent
-   [ ] Detect mixed requests
-   [ ] Extract dates
-   [ ] Extract nights
-   [ ] Extract guest count
-   [ ] Extract location
-   [ ] Extract room preference
-   [ ] Extract amenities
-   [ ] Extract maximum price

## 13.2 Agent routing

-   [ ] Route inventory task to Booking & Inventory Agent
-   [ ] Route policy task to Policy & Concierge Agent
-   [ ] Route document/search task to Search & RAG Agent
-   [ ] Support multiple sub-tasks
-   [ ] Execute independent tasks concurrently where appropriate

## 13.3 Result synthesis

-   [ ] Aggregate worker results
-   [ ] Resolve conflicting results
-   [ ] Preserve grounded facts
-   [ ] Create coherent response
-   [ ] Create structured booking option
-   [ ] Ask for confirmation before final reservation

------------------------------------------------------------------------

# 14. End-to-End Agentic Flow

Implement the exact project flow:

``` text
Guest
  ↓
MVC Chat UI
  ↓
POST /api/v1/chat/stream
  ↓
Master Orchestrator
  ↓
Intent Decomposition
  ├───────────────┐
  ↓               ↓
Booking Agent     Policy Agent
  ↓               ↓
Inventory         Native RAG
  ↓               ↓
Pricing           Policy Facts
  └───────┬───────┘
          ↓
Master Orchestrator
          ↓
Booking Summary
          ↓
User Confirmation
          ↓
Booking Plugin
          ↓
Inventory Lock
          ↓
Booking Voucher
          ↓
Streaming Response
```

Tasks:

-   [ ] Implement natural-language search
-   [ ] Implement mixed search + policy question
-   [ ] Implement availability lookup
-   [ ] Implement price calculation
-   [ ] Implement recommendation
-   [ ] Implement confirmation step
-   [ ] Implement reservation
-   [ ] Implement confirmation response

------------------------------------------------------------------------

# 15. Expected Scenario

The final system must handle a request similar to:

> "I want to book a luxury beachfront villa with a private pool for 3
> nights starting October 12th for 2 guests near Goa, under \$500/night.
> Also, what is the resort's policy on late check-in and pet
> accommodations?"

Expected behavior:

-   [ ] Identify Goa
-   [ ] Identify beachfront villa
-   [ ] Identify private pool requirement
-   [ ] Identify 3-night stay
-   [ ] Identify 2 guests
-   [ ] Identify \$500/night ceiling
-   [ ] Search available inventory
-   [ ] Calculate price
-   [ ] Search late check-in policy through RAG
-   [ ] Search pet policy through RAG
-   [ ] Combine results
-   [ ] Present recommended option
-   [ ] Ask user whether to hold/process reservation
-   [ ] Wait for confirmation
-   [ ] Execute reservation
-   [ ] Lock inventory
-   [ ] Generate booking voucher
-   [ ] Return confirmation

The SRS's example recommendation uses Azure Lagoon Ocean Resort, Royal
Beachfront Pool Villa, \$420/night and a 3-night total of \$1,260 before
taxes. The example policy answers include late check-in support and a
pet rule, but in the implementation these facts must come from the
loaded RAG documents rather than being hard-coded into the agent
response.

------------------------------------------------------------------------

# 16. API Layer

## 16.1 Resort APIs

-   [x] `GET /api/v1/resorts`
-   [x] `GET /api/v1/resorts/{id}`
-   [x] `GET /api/v1/resorts/search`

## 16.2 Inventory APIs

-   [x] `GET /api/v1/inventory/availability`
-   [ ] Verify availability behavior
-   [ ] Add reservation/lock endpoint if needed
-   [ ] Add inventory diagnostics only for development

## 16.3 Booking APIs

-   [ ] `POST /api/v1/booking/confirm`
-   [ ] Validate confirmation payload
-   [ ] Return booking voucher
-   [ ] Handle unavailable inventory
-   [ ] Handle invalid dates
-   [ ] Handle pricing errors

## 16.4 Chat API

-   [ ] Create `ChatController`
-   [ ] `POST /api/v1/chat/stream`
-   [ ] Accept user message
-   [ ] Accept session ID
-   [ ] Send request to Master Orchestrator
-   [ ] Stream response
-   [ ] Stream tool execution events
-   [ ] Stream agent execution status

## 16.5 Streaming

The SRS calls for SSE / SignalR for token-by-token responses and tool
telemetry.

-   [ ] Decide on SSE as initial implementation
-   [ ] Configure SSE response
-   [ ] Stream text chunks
-   [ ] Stream agent status events
-   [ ] Stream tool-call events
-   [ ] Stream errors
-   [ ] Stream final booking card payload
-   [ ] Verify browser/client consumption

------------------------------------------------------------------------

# 17. ASP.NET Core MVC Frontend

The SRS requires an ASP.NET Core 10 MVC presentation layer with Razor
views and interactive conversational UI.

## 17.1 MVC project

-   [ ] Create `OnlineResortBooking.Web`
-   [ ] Configure ASP.NET Core 10 MVC
-   [ ] Configure API base URL
-   [ ] Configure HTTP client
-   [ ] Configure dependency injection
-   [ ] Add shared layout

## 17.2 Chat UI

-   [ ] Create chat page
-   [ ] User message bubble
-   [ ] Assistant message bubble
-   [ ] Loading indicator
-   [ ] Streaming text rendering
-   [ ] Session handling
-   [ ] Error handling

## 17.3 Resort cards

-   [ ] Create resort option model
-   [ ] Create card component
-   [ ] Display resort
-   [ ] Display room type
-   [ ] Display amenities
-   [ ] Display private-pool indicator
-   [ ] Display price
-   [ ] Display dates
-   [ ] Add booking/select action

## 17.4 Agent status UI

-   [ ] Show Master Orchestrator status
-   [ ] Show Booking Agent status
-   [ ] Show Search/RAG Agent status
-   [ ] Show Policy Agent status
-   [ ] Show tool execution
-   [ ] Show completion/error state

## 17.5 Reservation summary

-   [ ] Create booking summary sidebar/card
-   [ ] Display guest details
-   [ ] Display dates
-   [ ] Display nights
-   [ ] Display room
-   [ ] Display nightly price
-   [ ] Display subtotal
-   [ ] Display taxes/fees
-   [ ] Display total
-   [ ] Confirm booking button
-   [ ] Display voucher after successful booking

## 17.6 View Components

-   [ ] Chat component
-   [ ] Agent execution status component
-   [ ] Resort card component
-   [ ] Reservation summary component

------------------------------------------------------------------------

# 18. Administrator Features

The SRS identifies a Resort Administrator as an actor.

Keep this lightweight because the project is an in-memory demonstration.

## 18.1 Document management

-   [ ] Create admin page
-   [ ] Load policy documents
-   [ ] Trigger RAG ingestion
-   [ ] Display loaded document count
-   [ ] Display chunk count
-   [ ] Allow re-indexing

## 18.2 Inventory administration

-   [ ] View room inventory
-   [ ] View current reservations
-   [ ] View room availability
-   [ ] Allow controlled inventory updates if useful

## 18.3 Telemetry

-   [ ] Display recent agent executions
-   [ ] Display tool calls
-   [ ] Display execution duration
-   [ ] Display success/failure
-   [ ] Display session/request ID

------------------------------------------------------------------------

# 19. Testing

The SRS explicitly calls for unit, integration, multi-agent, RAG
precision/recall and performance/concurrency testing.

## 19.1 Test projects

-   [ ] Create `OnlineResortBooking.Tests`
-   [ ] Configure xUnit
-   [ ] Add Moq
-   [ ] Add FluentAssertions if useful

## 19.2 Domain tests

-   [ ] DateRange rejects invalid range
-   [ ] DateRange calculates nights
-   [ ] MonetaryAmount rejects negative amount
-   [ ] Resort adds room types
-   [ ] BookingReservation creation
-   [ ] GuestProfile validation

## 19.3 Inventory tests

-   [ ] Room available initially
-   [ ] Room unavailable after capacity reached
-   [ ] Overlapping date detection
-   [ ] Adjacent dates allowed
-   [ ] Release works
-   [ ] Invalid room ID handled
-   [ ] Concurrent reservations cannot overbook

## 19.4 Pricing tests

-   [ ] Basic total
-   [ ] Multiple nights
-   [ ] Seasonal pricing
-   [ ] Taxes/fees
-   [ ] Price ceiling behavior

## 19.5 Plugin tests

-   [ ] InventoryPlugin
-   [ ] PricingPlugin
-   [ ] Search/RAG plugin
-   [ ] Policy plugin
-   [ ] Booking plugin

## 19.6 RAG tests

-   [ ] Chunking
-   [ ] Chunk overlap
-   [ ] Embedding generation
-   [ ] Cosine similarity
-   [ ] Top-K retrieval
-   [ ] Relevance threshold
-   [ ] Grounded answer
-   [ ] No-result behavior
-   [ ] Policy query precision
-   [ ] Policy query recall

## 19.7 Integration tests

-   [ ] API controller responses
-   [ ] Semantic Kernel tool invocation
-   [ ] Azure OpenAI integration
-   [ ] RAG retrieval
-   [ ] Booking workflow
-   [ ] Inventory reservation
-   [ ] Booking confirmation

## 19.8 Multi-agent tests

-   [ ] Correct agent routing
-   [ ] Multiple-agent handoff
-   [ ] Parallel task execution
-   [ ] Result synthesis
-   [ ] Conflict resolution
-   [ ] Booking only after confirmation
-   [ ] Agent failure handling

## 19.9 Performance/concurrency

-   [ ] Concurrent inventory reads
-   [ ] Concurrent reservations
-   [ ] Vector search performance
-   [ ] Concurrent chat sessions
-   [ ] Tool execution under load
-   [ ] Memory usage observation

------------------------------------------------------------------------

# 20. Security & Reliability

Even though this is an in-memory demo, keep enterprise-quality
boundaries.

-   [ ] Never hard-code Azure OpenAI API keys
-   [ ] Use configuration/environment secrets
-   [ ] Validate all API inputs
-   [ ] Validate tool parameters
-   [ ] Prevent AI from directly mutating state without tools
-   [ ] Require explicit booking confirmation
-   [ ] Prevent duplicate reservation execution
-   [ ] Handle unavailable inventory safely
-   [ ] Handle Azure OpenAI failures
-   [ ] Handle malformed model tool calls
-   [ ] Add request/session correlation IDs
-   [ ] Add structured logging
-   [ ] Avoid leaking secrets in logs

------------------------------------------------------------------------

# 21. Observability

-   [ ] Add structured logging
-   [ ] Add request correlation ID
-   [ ] Log agent start/end
-   [ ] Log tool start/end
-   [ ] Log RAG retrieval count
-   [ ] Log similarity scores
-   [ ] Log booking workflow steps
-   [ ] Log failures
-   [ ] Avoid logging sensitive credentials

------------------------------------------------------------------------

# 22. Final Demo Flow

Before declaring the project complete, run this exact scenario from the
MVC UI:

``` text
User:
"I want to book a luxury beachfront villa with a private pool
for 3 nights starting October 12th for 2 guests near Goa,
under $500/night. Also, what is the resort's policy on late
check-in and pet accommodations?"
```

Verify:

-   [ ] MVC sends the message
-   [ ] API receives it
-   [ ] Master Orchestrator identifies multiple intents
-   [ ] Booking Agent handles inventory/pricing
-   [ ] Search/RAG or Policy Agent handles policy questions
-   [ ] Agents execute required tools
-   [ ] RAG returns grounded policy information
-   [ ] Pricing is calculated deterministically
-   [ ] Recommendation is produced
-   [ ] User receives a booking summary
-   [ ] User confirms booking
-   [ ] Booking Plugin executes
-   [ ] Inventory is locked
-   [ ] Voucher is generated
-   [ ] Confirmation is streamed to UI
-   [ ] Agent/tool telemetry appears in UI

------------------------------------------------------------------------

# 23. Final Architecture Review

Before submission/demo:

-   [ ] Domain has no infrastructure dependency
-   [ ] Application owns abstractions/use cases
-   [ ] Infrastructure implements persistence/runtime services
-   [ ] API only handles HTTP concerns
-   [ ] MVC only handles presentation concerns
-   [ ] AI agents do not contain core booking business rules
-   [ ] Deterministic operations are implemented as tools/services
-   [ ] No persistent database exists
-   [ ] Runtime state uses appropriate in-memory/thread-safe structures
-   [ ] RAG is native/in-memory
-   [ ] Semantic Kernel is integrated cleanly
-   [ ] Azure OpenAI is isolated behind configuration/services
-   [ ] Booking requires explicit confirmation
-   [ ] Tests cover critical workflows

------------------------------------------------------------------------

# 24. Current Progress

## Completed

-   [x] Solution created
-   [x] Clean Architecture projects created
-   [x] Project references configured
-   [x] Domain entities implemented
-   [x] Domain value objects implemented
-   [x] Resort repository implemented
-   [x] Resort seed data implemented
-   [x] Resort API implemented
-   [x] Inventory contract implemented
-   [x] In-memory inventory implemented
-   [x] Inventory seeded
-   [x] Shared repository dependency fixed
-   [x] Availability API implemented
-   [x] Pricing service implemented
-   [x] Multiple successful `dotnet build` checks

## Completed but needs verification/follow-up

-   \[\~\] Availability endpoint --- currently returning `false`
    unexpectedly; intentionally deferred for later debugging.

## Current position

**Next major implementation target:**

``` text
Booking Application Service
        ↓
Booking Plugin
        ↓
Semantic Kernel
        ↓
Native RAG
        ↓
Specialized Agents
        ↓
Master Orchestrator
        ↓
Chat API + Streaming
        ↓
MVC Frontend
        ↓
Tests + Final Demo
```

------------------------------------------------------------------------

# 25. Recommended Implementation Order

Follow this order rather than jumping directly into multi-agent AI:

1.  [x] Architecture
2.  [x] Domain
3.  [x] Resort repository/API
4.  [x] Inventory foundation
5.  [x] Basic pricing
6.  [ ] Complete deterministic booking workflow
7.  [ ] Booking/inventory/pricing tools
8.  [ ] Native RAG
9.  [ ] Semantic Kernel foundation
10. [ ] Semantic Kernel plugins
11. [ ] Single-agent tool-calling workflow
12. [ ] Search & RAG Agent
13. [ ] Booking & Inventory Agent
14. [ ] Policy & Concierge Agent
15. [ ] Master Orchestrator
16. [ ] Conversation/session memory
17. [ ] Chat API
18. [ ] SSE streaming
19. [ ] MVC frontend
20. [ ] Admin features
21. [ ] Unit tests
22. [ ] Integration tests
23. [ ] Multi-agent tests
24. [ ] RAG evaluation
25. [ ] Concurrency/performance tests
26. [ ] End-to-end demo
27. [ ] Architecture/code cleanup
28. [ ] Final documentation

------------------------------------------------------------------------

## Definition of Done

The project is complete when a user can start from the MVC chat UI, make
the natural-language resort request, receive an AI-generated
recommendation grounded in actual inventory/pricing/RAG data, ask policy
questions, confirm the booking, and receive a reservation voucher ---
with the underlying execution demonstrating Clean Architecture, Semantic
Kernel plugins, native RAG, specialized agents, Master Orchestration,
in-memory inventory, and streaming responses.
