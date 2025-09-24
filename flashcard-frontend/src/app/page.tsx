import FlashcardApp from "@/components/flashcard/FlashcardApp";

export default function FlashcardPage() {
  return (
    <div className="min-h-screen bg-background flex flex-col items-center justify-center p-4">
      <div className="w-full max-w-2xl mx-auto space-y-8">
        <div className="text-center space-y-2">
          <h1 className="text-4xl font-bold text-foreground text-balance">
            Study Flashcards
          </h1>
          <p className="text-muted-foreground text-lg">
            Click the card or use the button to reveal answers
          </p>
        </div>

        <FlashcardApp />
      </div>
    </div>
  );
}
