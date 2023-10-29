using FlashCardDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashCardDemo.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        FlashcardDbContext context = serviceScope.ServiceProvider.GetRequiredService<FlashcardDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        /* Folder seed */
        if (!context.Folders.Any())
        {
            var folders = new List<Folder>
            {
                new Folder
                {
                    FolderName = "DemoFolder1",
                    FolderDescription = "Amidst the bustling city, a solitary bench by the tranquil pond offers a peaceful escape from the urban chaos.",
                    CreationDate = DateTime.UtcNow
                }
            };
            context.AddRange(folders);
            context.SaveChanges();
        }

        /*

         */
        /* Deck seed */
        if (!context.Decks.Any())
        {
            var decks = new List<Deck>
            {
                new Deck
                {
                    DeckName = "Food",
                    DeckDescription = "One cannot think well, love well, sleep well, if one has not dined well.",
                    FolderId = 1,
                    CreationDate = DateTime.UtcNow
                },
                new Deck
                {
                    DeckName = "History",
                    DeckDescription = "A generation which ignores history has no past and no future.",
                    CreationDate = DateTime.UtcNow
                },
            };
            context.AddRange(decks);
            context.SaveChanges();
        }

        /* Flashcard seed */
        if (!context.Flashcards.Any())
        {
            var flashcards = new List<Flashcard>
            {
                    new Flashcard
                    {
                        Question = "What is a Pizza?",
                        Answer = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is a Fried Chicken Leg?",
                        Answer = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is French Fries?",
                        Answer = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is Grilled Ribs?",
                        Answer = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1
                    },
                    new Flashcard
                    {
                        Question = "What is Tacos?",
                        Answer = "Tortillas filled with various ingredients such as meat, vegetables, and salsa, folded into a delicious meal.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is Fish and Chips?",
                        Answer = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is a Cider?",
                        Answer = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "What is a Coke?",
                        Answer = "Popular carbonated soft drink known for its sweet and refreshing taste. A short nickname for 'Cocaine'.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 1,
                    },
                    new Flashcard
                    {
                        Question = "Who was the first Emperor of Rome?",
                        Answer = "Augustus, formerly known as Octavian, was the first Emperor of Rome.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "Who wrote \"The Communist Manifesto\"?",
                        Answer = "Karl Marx and Friedrich Engels co-authored \"The Communist Manifesto\" in 1848.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "What was the primary cause of the Civil War in the United States?",
                        Answer = "The primary cause of the American Civil War was the issue of slavery.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "Who was the leader of the Nazi Party in Germany during World War II?",
                        Answer = "Adolf Hitler was the leader of the Nazi Party in Germany during World War II.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "What event is often considered the beginning of the Great Depression in the United States?",
                        Answer = "The stock market crash of 1929 is often seen as the beginning of the Great Depression.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "When did the Berlin Wall fall, leading to the reunification of Germany?",
                        Answer = "The Berlin Wall fell on November 9, 1989, leading to the reunification of East and West Germany.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "When was the American Declaration of Independence adopted?",
                        Answer = "The American Declaration of Independence was adopted on July 4, 1776.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },
                    new Flashcard
                    {
                        Question = "When was the Apollo program by NASA?",
                        Answer = "The NASA's Apollo program was achieved in 1969.",
                        CreationDate = DateTime.UtcNow,
                        DeckId = 2,
                    },


            };

            context.AddRange(flashcards);
            context.SaveChanges();
        }

    }
}
