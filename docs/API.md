# NeuroPath API reference

Every route is under `/api`. "Signed in" means any valid access token; roles are `Admin`, `Trainer` and `User`. Errors are returned as RFC 7807 problem details with a `traceId`.

Interactive documentation is served at `/swagger` in the Development environment.

## Health

| Method | Route | Access | Purpose |
|---|---|---|---|
| GET | `/health` | Anyone | Liveness: the process is running |
| GET | `/health/ready` | Anyone | Readiness: the database is reachable |

## `/api/feedback`

| Method | Route | Access |
|---|---|---|
| POST | `/api/feedback` | Signed in |
| GET | `/api/feedback` | Admin |
| POST | `/api/feedback/{id}/respond` | Admin |

## `/api/reports`

| Method | Route | Access |
|---|---|---|
| POST | `/api/reports` | Signed in |
| GET | `/api/reports` | Admin |
| PUT | `/api/reports/{id}/review` | Admin |

## `/api/support-tickets`

| Method | Route | Access |
|---|---|---|
| GET | `/api/support-tickets/mine` | Signed in |
| POST | `/api/support-tickets` | Signed in |
| GET | `/api/support-tickets` | Admin |
| PUT | `/api/support-tickets/{id}/assign` | Admin |
| PUT | `/api/support-tickets/{id}/resolve` | Admin |

## `/api/activity-logs`

| Method | Route | Access |
|---|---|---|
| GET | `/api/activity-logs` | Admin |

## `/api/auth`

| Method | Route | Access |
|---|---|---|
| POST | `/api/auth/register` | Anyone |
| POST | `/api/auth/login` | Anyone |
| POST | `/api/auth/refresh` | Anyone |
| POST | `/api/auth/verify-email` | Anyone |
| POST | `/api/auth/resend-verification` | Anyone |
| POST | `/api/auth/forgot-password` | Anyone |
| POST | `/api/auth/reset-password` | Anyone |
| POST | `/api/auth/change-password` | Signed in |
| POST | `/api/auth/logout` | Signed in |

## `/api/brain-areas`

| Method | Route | Access |
|---|---|---|
| GET | `/api/brain-areas` | Anyone |
| GET | `/api/brain-areas/{id}` | Anyone |
| POST | `/api/brain-areas` | Admin |
| PUT | `/api/brain-areas/{id}` | Admin |
| DELETE | `/api/brain-areas/{id}` | Admin |

## `/api/brain-regions`

| Method | Route | Access |
|---|---|---|
| GET | `/api/brain-regions` | Anyone |
| GET | `/api/brain-regions/{id}` | Anyone |
| POST | `/api/brain-regions` | Admin |

## `/api/neurotransmitters`

| Method | Route | Access |
|---|---|---|
| GET | `/api/neurotransmitters` | Anyone |
| GET | `/api/neurotransmitters/{id}` | Anyone |

## `/api/cognitive-skills`

| Method | Route | Access |
|---|---|---|
| GET | `/api/cognitive-skills` | Anyone |
| GET | `/api/cognitive-skills/{id}` | Anyone |

## `/api/articles`

| Method | Route | Access |
|---|---|---|
| GET | `/api/articles` | Anyone |
| GET | `/api/articles/featured` | Anyone |
| GET | `/api/articles/categories` | Anyone |
| GET | `/api/articles/categories/{categoryId}` | Anyone |
| GET | `/api/articles/{id}` | Anyone |
| POST | `/api/articles` | Admin, Trainer |
| PUT | `/api/articles/{id}` | Admin, Trainer |
| POST | `/api/articles/{id}/publish` | Admin, Trainer |
| DELETE | `/api/articles/{id}` | Admin, Trainer |
| GET | `/api/articles/{id}/comments` | Anyone |

## `/api/comments`

| Method | Route | Access |
|---|---|---|
| POST | `/api/comments` | Signed in |
| PUT | `/api/comments/{id}` | Signed in |
| DELETE | `/api/comments/{id}` | Signed in |
| POST | `/api/comments/{id}/like` | Signed in |
| DELETE | `/api/comments/{id}/like` | Signed in |

## `/api/bookmarks`

| Method | Route | Access |
|---|---|---|
| GET | `/api/bookmarks` | Signed in |
| POST | `/api/bookmarks` | Signed in |
| DELETE | `/api/bookmarks/{id}` | Signed in |

## `/api/daily-tips`

| Method | Route | Access |
|---|---|---|
| GET | `/api/daily-tips/today` | Anyone |
| GET | `/api/daily-tips` | Admin |
| POST | `/api/daily-tips` | Admin |
| PUT | `/api/daily-tips/{id}` | Admin |
| DELETE | `/api/daily-tips/{id}` | Admin |

## `/api/tags`

| Method | Route | Access |
|---|---|---|
| GET | `/api/tags` | Anyone |
| POST | `/api/tags` | Admin, Trainer |

## `/api/faq`

| Method | Route | Access |
|---|---|---|
| GET | `/api/faq` | Anyone |
| GET | `/api/faq/categories` | Anyone |
| POST | `/api/faq` | Admin |
| PUT | `/api/faq/{id}` | Admin |
| DELETE | `/api/faq/{id}` | Admin |

## `/api/exercises`

| Method | Route | Access |
|---|---|---|
| GET | `/api/exercises` | Anyone |
| GET | `/api/exercises/{id}` | Anyone |
| GET | `/api/exercises/categories` | Anyone |
| GET | `/api/exercises/difficulty-levels` | Anyone |
| POST | `/api/exercises` | Admin, Trainer |
| PUT | `/api/exercises/{id}` | Admin, Trainer |
| DELETE | `/api/exercises/{id}` | Admin, Trainer |
| GET | `/api/exercises/{id}/hints` | Signed in |
| POST | `/api/exercises/{id}/hints` | Admin, Trainer |
| PUT | `/api/exercises/{id}/hints/{hintId}` | Admin, Trainer |
| DELETE | `/api/exercises/{id}/hints/{hintId}` | Admin, Trainer |
| GET | `/api/exercises/{id}/reviews` | Anyone |
| POST | `/api/exercises/{id}/reviews` | Signed in |
| PUT | `/api/exercises/reviews/{reviewId}/approve` | Admin |

## `/api/files`

| Method | Route | Access |
|---|---|---|
| GET | `/api/files` | Signed in |
| POST | `/api/files/avatar` | Signed in |
| POST | `/api/files/exercise-image` | Admin, Trainer |
| POST | `/api/files/article-image` | Admin, Trainer |
| POST | `/api/files/certificate-pdf` | Admin |
| DELETE | `/api/files/{id}` | Signed in |

## `/api/achievements`

| Method | Route | Access |
|---|---|---|
| GET | `/api/achievements` | Anyone |
| GET | `/api/achievements/mine` | Signed in |
| POST | `/api/achievements` | Admin |
| PUT | `/api/achievements/{id}` | Admin |
| DELETE | `/api/achievements/{id}` | Admin |

## `/api/leaderboard`

| Method | Route | Access |
|---|---|---|
| GET | `/api/leaderboard` | Anyone |
| POST | `/api/leaderboard/refresh` | Admin |

## `/api/shop`

| Method | Route | Access |
|---|---|---|
| GET | `/api/shop/items` | Anyone |
| GET | `/api/shop/items/all` | Admin |
| POST | `/api/shop/items` | Admin |
| PUT | `/api/shop/items/{itemId}` | Admin |
| POST | `/api/shop/items/{itemId}/purchase` | Signed in |
| GET | `/api/shop/inventory` | Signed in |
| PUT | `/api/shop/inventory/{inventoryId}/equipped` | Signed in |
| GET | `/api/shop/streak` | Signed in |

## `/api/progress`

| Method | Route | Access |
|---|---|---|
| GET | `/api/progress/summary` | Signed in |
| GET | `/api/progress/brain-areas` | Signed in |
| GET | `/api/progress/snapshots` | Signed in |
| POST | `/api/progress/snapshots` | Signed in |

## `/api/challenges`

| Method | Route | Access |
|---|---|---|
| GET | `/api/challenges` | Anyone |
| GET | `/api/challenges/{id}` | Anyone |
| GET | `/api/challenges/{id}/leaderboard` | Anyone |
| POST | `/api/challenges` | Admin |
| PUT | `/api/challenges/{id}` | Admin |
| PUT | `/api/challenges/{id}/active` | Admin |
| POST | `/api/challenges/{id}/join` | Signed in |
| POST | `/api/challenges/{id}/teams/{teamId}/join` | Signed in |

## `/api/courses`

| Method | Route | Access |
|---|---|---|
| GET | `/api/courses` | Anyone |
| GET | `/api/courses/{id}` | Anyone |
| POST | `/api/courses` | Admin, Trainer |
| PUT | `/api/courses/{id}` | Admin, Trainer |
| DELETE | `/api/courses/{id}` | Admin, Trainer |
| POST | `/api/courses/{id}/publish` | Admin, Trainer |
| POST | `/api/courses/{id}/lessons` | Admin, Trainer |
| PUT | `/api/courses/{id}/lessons/{lessonId}` | Admin, Trainer |
| DELETE | `/api/courses/{id}/lessons/{lessonId}` | Admin, Trainer |
| POST | `/api/courses/{id}/enroll` | Signed in |
| POST | `/api/courses/{id}/lessons/{lessonId}/complete` | Signed in |
| GET | `/api/courses/progress` | Signed in |

## `/api/assessments`

| Method | Route | Access |
|---|---|---|
| GET | `/api/assessments` | Anyone |
| GET | `/api/assessments/{id}` | Signed in |
| POST | `/api/assessments` | Admin |
| PUT | `/api/assessments/{id}` | Admin |
| DELETE | `/api/assessments/{id}` | Admin |
| POST | `/api/assessments/{id}/questions` | Admin |
| POST | `/api/assessments/{id}/submit` | Signed in |
| GET | `/api/assessments/results` | Signed in |

## `/api/certificates`

| Method | Route | Access |
|---|---|---|
| GET | `/api/certificates` | Anyone |
| GET | `/api/certificates/mine` | Signed in |
| POST | `/api/certificates` | Admin |
| PUT | `/api/certificates/{id}` | Admin |

## `/api/quizzes`

| Method | Route | Access |
|---|---|---|
| GET | `/api/quizzes` | Anyone |
| GET | `/api/quizzes/{id}` | Signed in |
| POST | `/api/quizzes` | Admin, Trainer |
| POST | `/api/quizzes/{id}/questions` | Admin, Trainer |
| PUT | `/api/quizzes/{id}` | Admin, Trainer |
| PUT | `/api/quizzes/{id}/questions/{questionId}` | Admin, Trainer |
| DELETE | `/api/quizzes/{id}/questions/{questionId}` | Admin, Trainer |
| POST | `/api/quizzes/{id}/unpublish` | Admin, Trainer |
| POST | `/api/quizzes/{id}/publish` | Admin, Trainer |
| DELETE | `/api/quizzes/{id}` | Admin, Trainer |
| POST | `/api/quizzes/{id}/start` | Signed in |
| POST | `/api/quizzes/submit` | Signed in |
| GET | `/api/quizzes/history` | Signed in |

## `/api/friends`

| Method | Route | Access |
|---|---|---|
| GET | `/api/friends` | Signed in |
| GET | `/api/friends/requests` | Signed in |
| POST | `/api/friends/requests` | Signed in |
| POST | `/api/friends/requests/{requestId}/accept` | Signed in |
| POST | `/api/friends/requests/{requestId}/reject` | Signed in |
| DELETE | `/api/friends/{friendId}` | Signed in |

## `/api/messages`

| Method | Route | Access |
|---|---|---|
| GET | `/api/messages/conversations` | Signed in |
| GET | `/api/messages/conversations/{otherUserId}` | Signed in |
| POST | `/api/messages` | Signed in |
| POST | `/api/messages/{id}/read` | Signed in |

## `/api/teams`

| Method | Route | Access |
|---|---|---|
| GET | `/api/teams` | Anyone |
| GET | `/api/teams/{id}` | Anyone |
| GET | `/api/teams/{id}/members` | Anyone |
| POST | `/api/teams` | Signed in |
| PUT | `/api/teams/{id}` | Signed in |
| DELETE | `/api/teams/{id}` | Signed in |
| DELETE | `/api/teams/{id}/members/{userId}` | Signed in |
| POST | `/api/teams/{id}/join` | Signed in |
| POST | `/api/teams/{id}/leave` | Signed in |
| PUT | `/api/teams/{id}/members/{userId}/role` | Signed in |

## `/api/training-plans`

| Method | Route | Access |
|---|---|---|
| GET | `/api/training-plans` | Signed in |
| GET | `/api/training-plans/{id}` | Signed in |
| POST | `/api/training-plans` | Signed in |
| PUT | `/api/training-plans/{id}` | Signed in |
| DELETE | `/api/training-plans/{id}` | Signed in |
| POST | `/api/training-plans/{id}/exercises` | Signed in |
| DELETE | `/api/training-plans/exercises/{planExerciseId}` | Signed in |

## `/api/sessions`

| Method | Route | Access |
|---|---|---|
| GET | `/api/sessions` | Signed in |
| POST | `/api/sessions` | Signed in |
| POST | `/api/sessions/{id}/complete` | Signed in |
| POST | `/api/sessions/{id}/exercises` | Signed in |
| GET | `/api/sessions/logs` | Signed in |
| GET | `/api/sessions/stats` | Signed in |

## `/api/users`

| Method | Route | Access |
|---|---|---|
| GET | `/api/users/me` | Signed in |
| GET | `/api/users/me/profile` | Signed in |
| PUT | `/api/users/me/profile` | Signed in |
| GET | `/api/users/me/settings` | Signed in |
| PUT | `/api/users/me/settings` | Signed in |
| DELETE | `/api/users/me` | Signed in |
| GET | `/api/users` | Admin |
| GET | `/api/users/{id}` | Admin |
| PUT | `/api/users/{id}/blocked` | Admin |
| PUT | `/api/users/{id}/role` | Admin |

## `/api/mood-logs`

| Method | Route | Access |
|---|---|---|
| GET | `/api/mood-logs` | Signed in |
| POST | `/api/mood-logs` | Signed in |
| DELETE | `/api/mood-logs/{id}` | Signed in |
| GET | `/api/mood-logs/stats` | Signed in |

## `/api/notifications`

| Method | Route | Access |
|---|---|---|
| GET | `/api/notifications` | Signed in |
| GET | `/api/notifications/unread-count` | Signed in |
| POST | `/api/notifications/{id}/read` | Signed in |
| POST | `/api/notifications/read-all` | Signed in |
| DELETE | `/api/notifications/{id}` | Signed in |
