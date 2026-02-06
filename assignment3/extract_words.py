import re


# opens text file and makes a list of each word
# due to removing white spaces and special characters some single letter words exist "r" or "d"
def generate_wordlist(filename):
    wordlist = []
    with open(filename, "r", encoding="utf-8") as f:
        for line in f:
            line = line.strip()
            line = line.lower()
            line = re.findall('[a-z]+', line)
            for word in line:
                wordlist.append(word)
    return wordlist


def gen_files(wordlist):
    # code for all words file
    with open("allwords.txt", "w") as f:
        for word in wordlist:
            f.write(f"{word}\n")

    # generates dictionary of word:frequency
    word_frequency = {}
    for word in wordlist:
        if word in word_frequency:
            word_frequency[word] += 1
        else:
            word_frequency[word] = 1
    
    # code for unique words file
    unique_words = []
    for word, freq in word_frequency.items():
        if freq == 1:
            unique_words.append(word)
    with open("uniquewords.txt", "w") as f:
        for word in unique_words:
            f.write(f"{word}\n")

    # code for wordfrequency file
    frequency_num = {} # frequency: count of words
    for freq in word_frequency.values():
        if freq in frequency_num.keys():
            frequency_num[freq] += 1
        else:
            frequency_num[freq] = 1
    freq_list = [(freq, count) for freq, count in frequency_num.items()] # makes list of tuples of (frequency, count of words)
    freq_list.sort(reverse=False, key=lambda x: x[0]) # sort by frequency
    with open("wordfrequency.txt", "w") as f:
        for freq, count in freq_list:
            f.write(f"{freq}:{count}\n")


def main():
    filename = "DollsHouse.txt"
    wordlist = generate_wordlist(filename)
    gen_files(wordlist)


if __name__ == "__main__":
    main()
